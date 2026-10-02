using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using System.IO;
using System.Text;

namespace XRMultiplayer
{
    public class SessionDataManager : MonoBehaviour
    {
        public static SessionDataManager Instance { get; private set; }

        [System.Serializable]
        public class FrameData
        {
            public float timestamp;
            public Vector3 playerPosition;
            public Vector3 playerRotation;
            public float playerSpeed;
            public float rotationSpeed;
        }

        [System.Serializable]
        public class EventData
        {
            public float timestamp;
            public string eventType;
            public string description;
        }

        private string patientID;
        private string currentScenario;
        private float sessionStartTime;
        private List<FrameData> frameDataList = new List<FrameData>();
        private List<EventData> eventDataList = new List<EventData>();
        private Vector3 lastPlayerPosition = Vector3.zero;
        private Vector3 lastPlayerRotation = Vector3.zero;
        private float lastFrameRecordTime = 0f;
        private const float RECORD_INTERVAL = 0.1f; // 0.1초 단위
        private StreamWriter debugLogWriter;
        private string debugLogPath;

        // Calibration data
        private Vector3 calibrationYawAngle;
        private Vector3 calibrationPositionOffset;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;

                InitializeDebugLog();

                // Auto-generate Patient ID
                if (string.IsNullOrEmpty(patientID))
                {
                    patientID = "P_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    DebugLog($"[SessionDataManager] Auto-generated Patient ID: {patientID}");
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            // Must unsubscribe here, not only in OnApplicationQuit. If this object is
            // destroyed while still subscribed, the delegate keeps the C# object alive:
            // Update() stops running (no frames recorded) but OnSceneLoaded keeps firing,
            // so every later scene change writes an empty CSV. Unity also reports the
            // destroyed instance as == null, letting a second manager claim Instance and
            // generate a second patient ID for the same run.
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (Instance == this) Instance = null;

            if (debugLogWriter != null)
            {
                debugLogWriter.Close();
                debugLogWriter.Dispose();
                debugLogWriter = null;
            }
        }

        void OnApplicationQuit()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                if (debugLogWriter != null)
                {
                    debugLogWriter.Close();
                    debugLogWriter.Dispose();
                    debugLogWriter = null;
                }
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string sceneName = scene.name;

            // End previous session if returning to menu or loading a new scenario
            if (!string.IsNullOrEmpty(currentScenario) &&
                !(sceneName.Contains("Walkway") || sceneName.Contains("walkway")))
            {
                DebugLog($"[SessionDataManager] Scene changed to {sceneName}, ending current session");
                EndSession();
            }

            // Auto-detect Walkways scenario and start session
            if (sceneName.Contains("Walkway") || sceneName.Contains("walkway"))
            {
                DebugLog($"[SessionDataManager] Walkway scene detected, starting session");
                StartSession("Walkways");
            }

            DebugLog($"[SessionDataManager] Scene loaded: {sceneName}");
        }

        void Update()
        {
            if (!string.IsNullOrEmpty(patientID) && !string.IsNullOrEmpty(currentScenario))
            {
                RecordFrameData();
            }
        }

        public int GetFrameDataCount()
        {
            return frameDataList.Count;
        }

        public void SetPatientID(string id)
        {
            patientID = id;
            Debug.Log($"[SessionDataManager] Patient ID set: {patientID}");
        }

        public void StartSession(string scenarioName)
        {
            currentScenario = scenarioName;
            sessionStartTime = Time.time;
            frameDataList.Clear();
            eventDataList.Clear();
            lastFrameRecordTime = sessionStartTime;

            DebugLog($"[SessionDataManager] Session started: {scenarioName} at {sessionStartTime}, Patient ID: {patientID}");
            LogEvent("SESSION_START", $"Scenario: {scenarioName}");
        }

        public void EndSession()
        {
            float sessionEndTime = Time.time;
            DebugLog($"[SessionDataManager] EndSession called, Patient ID: {patientID}, Scenario: {currentScenario}");
            LogEvent("SESSION_END", $"Duration: {sessionEndTime - sessionStartTime:F2}s");

            SaveSessionToCSV(sessionEndTime);
            currentScenario = "";

            DebugLog("[SessionDataManager] Session ended and saved");
        }

        public void RecordCalibration(Vector3 yawAngle, Vector3 positionOffset)
        {
            calibrationYawAngle = yawAngle;
            calibrationPositionOffset = positionOffset;

            LogEvent("CALIBRATION", $"Yaw: {yawAngle.y:F2}°, Offset: {positionOffset}");
            Debug.Log($"[SessionDataManager] Calibration recorded: Yaw={yawAngle.y:F2}°, Offset={positionOffset}");
        }

        public void LogEvent(string eventType, string description)
        {
            EventData evt = new EventData
            {
                timestamp = Time.time - sessionStartTime,
                eventType = eventType,
                description = description
            };
            eventDataList.Add(evt);
        }

        private void RecordFrameData()
        {
            float currentTime = Time.time;

            if (currentTime - lastFrameRecordTime >= RECORD_INTERVAL)
            {
                Transform cameraRig = FindCameraRig();
                if (cameraRig != null)
                {
                    Vector3 playerPos = cameraRig.position;
                    Vector3 playerRot = cameraRig.eulerAngles;

                    float speed = Vector3.Distance(playerPos, lastPlayerPosition) / RECORD_INTERVAL;
                    float rotSpeed = Vector3.Distance(playerRot, lastPlayerRotation) / RECORD_INTERVAL;

                    FrameData frame = new FrameData
                    {
                        timestamp = currentTime - sessionStartTime,
                        playerPosition = playerPos,
                        playerRotation = playerRot,
                        playerSpeed = speed,
                        rotationSpeed = rotSpeed
                    };

                    frameDataList.Add(frame);
                    lastPlayerPosition = playerPos;
                    lastPlayerRotation = playerRot;
                    lastFrameRecordTime = currentTime;
                }
                else
                {
                    if (frameDataList.Count == 0)
                    {
                        DebugLog("[SessionDataManager] WARNING: Camera Rig not found! Frame data will not be recorded.");
                    }
                }
            }
        }

        private Transform FindCameraRig()
        {
            // Try to find [BuildingBlock] Camera Rig
            GameObject cameraRigObj = GameObject.Find("[BuildingBlock] Camera Rig");
            if (cameraRigObj != null)
                return cameraRigObj.transform;

            // Fallback: Try to find by Camera.main
            if (Camera.main != null)
                return Camera.main.transform.parent ?? Camera.main.transform;

            return null;
        }

        private void SaveSessionToCSV(float sessionEndTime)
        {
            if (string.IsNullOrEmpty(patientID))
            {
                DebugLog("[SessionDataManager] WARNING: Patient ID not set, cannot save session");
                return;
            }

            if (frameDataList.Count == 0)
            {
                DebugLog($"[SessionDataManager] WARNING: 0 frames recorded for {currentScenario}, skipping save (no data to write)");
                return;
            }

            string fileName = $"FoG_{patientID}_{currentScenario}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            string filePath = Path.Combine(GetDataDirectory(), fileName);

            try
            {
                DebugLog($"[SessionDataManager] Attempting to save session to: {filePath}");
                DebugLog($"[SessionDataManager] Frame data count: {frameDataList.Count}, Event data count: {eventDataList.Count}");

                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Header
                    writer.WriteLine("=== Session Info ===");
                    writer.WriteLine($"Patient ID,{patientID}");
                    writer.WriteLine($"Scenario,{currentScenario}");
                    writer.WriteLine($"Session Duration,{sessionEndTime - sessionStartTime:F2}");
                    writer.WriteLine($"Start Time,{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine();

                    // Calibration Data
                    writer.WriteLine("=== Calibration Data ===");
                    writer.WriteLine($"Yaw Angle (Y),{calibrationYawAngle.y:F2}");
                    writer.WriteLine($"Position Offset,{calibrationPositionOffset}");
                    writer.WriteLine();

                    // Events
                    writer.WriteLine("=== Events ===");
                    writer.WriteLine("Timestamp,Event Type,Description");
                    foreach (var evt in eventDataList)
                    {
                        writer.WriteLine($"{evt.timestamp:F2},{evt.eventType},\"{evt.description}\"");
                    }
                    writer.WriteLine();

                    // Frame Data
                    writer.WriteLine("=== Frame Data ===");
                    writer.WriteLine("Timestamp,PosX,PosY,PosZ,RotX,RotY,RotZ,Speed,RotSpeed");
                    foreach (var frame in frameDataList)
                    {
                        writer.WriteLine($"{frame.timestamp:F2}," +
                            $"{frame.playerPosition.x:F3}," +
                            $"{frame.playerPosition.y:F3}," +
                            $"{frame.playerPosition.z:F3}," +
                            $"{frame.playerRotation.x:F1}," +
                            $"{frame.playerRotation.y:F1}," +
                            $"{frame.playerRotation.z:F1}," +
                            $"{frame.playerSpeed:F3}," +
                            $"{frame.rotationSpeed:F3}");
                    }
                }

                DebugLog($"[SessionDataManager] Session successfully saved to: {filePath}");
            }
            catch (System.Exception e)
            {
                DebugLog($"[SessionDataManager] ERROR saving session: {e.Message}");
            }
        }

        private string GetDataDirectory()
        {
            string dataPath = Path.Combine(
                Application.persistentDataPath,
                "FoG_Data"
            );

            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            return dataPath;
        }

        private void InitializeDebugLog()
        {
            try
            {
                debugLogPath = Path.Combine(GetDataDirectory(), "SessionDebugLog.txt");
                debugLogWriter = new StreamWriter(debugLogPath, true, Encoding.UTF8);
                debugLogWriter.AutoFlush = true;
                DebugLog("[SessionDataManager] Debug log initialized");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SessionDataManager] Failed to initialize debug log: {e.Message}");
            }
        }

        private void DebugLog(string message)
        {
            Debug.Log(message);
            if (debugLogWriter != null)
            {
                try
                {
                    debugLogWriter.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}");
                }
                catch { }
            }
        }
    }
}
