using System.Globalization;
using System.IO;
using System;
using UnityEngine;
using System.Text;

public class BodyKinematicsLogger : MonoBehaviour
{
    public double StartTimeRealtime { get { return startTime; } }

    [Header("Logging Settings")]
    public Transform head;
    public Transform hips;
    public Transform leftFoot;
    public Transform rightFoot;
    public float sampleRateHz = 15f;

    private string logPath;
    private StreamWriter writer;
    private double startTime;
    private double fixedDelta;
    private int frameIndex = 0;
    private bool initialized = false;

    private Vector3 prevHeadPos, prevHipsPos, prevLeftFootPos, prevRightFootPos;
    private Vector3 prevHeadVel, prevHipsVel, prevLeftFootVel, prevRightFootVel;

    // Only one logger may run at a time. Two loggers starting in the same second
    // would generate the same file name and overwrite each other's data.
    private static BodyKinematicsLogger activeLogger;

    void Start()
    {
        if (activeLogger != null && activeLogger != this)
        {
            Debug.LogWarning($"[BodyKinematicsLogger] Duplicate logger on '{gameObject.name}' disabled. '{activeLogger.gameObject.name}' is already logging. Remove the extra component from the scene.");
            enabled = false;
            return;
        }
        activeLogger = this;

        // Auto-find body parts if not assigned
        if (!head)
        {
            GameObject headObj = GameObject.Find("Head") ??
                                 GameObject.Find("head") ??
                                 GameObject.Find("[BuildingBlock] Camera Rig");
            if (headObj) head = headObj.transform;
            if (!head && Camera.main != null) head = Camera.main.transform;
        }

        if (!hips)
        {
            GameObject hipsObj = GameObject.Find("Hips") ?? GameObject.Find("hips");
            if (hipsObj) hips = hipsObj.transform;
        }

        if (!leftFoot)
        {
            GameObject leftFootObj = GameObject.Find("LeftFoot") ?? GameObject.Find("leftFoot") ??
                                     GameObject.Find("Left_Foot") ?? GameObject.Find("Left Foot");
            if (leftFootObj) leftFoot = leftFootObj.transform;
        }

        if (!rightFoot)
        {
            GameObject rightFootObj = GameObject.Find("RightFoot") ?? GameObject.Find("rightFoot") ??
                                      GameObject.Find("Right_Foot") ?? GameObject.Find("Right Foot");
            if (rightFootObj) rightFoot = rightFootObj.transform;
        }

        Debug.Log($"[BodyKinematicsLogger] Found - Head: {(head ? head.name : "NOT FOUND")}, Hips: {(hips ? hips.name : "NOT FOUND")}, LeftFoot: {(leftFoot ? leftFoot.name : "NOT FOUND")}, RightFoot: {(rightFoot ? rightFoot.name : "NOT FOUND")}");

        // Create folder
        string folder = Path.Combine(Application.persistentDataPath, "_LocalLogs");
        Directory.CreateDirectory(folder);

        // Create file with timestamp
        string fileName = $"BodyTracking_{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}.csv";
        logPath = Path.Combine(folder, fileName);

        // Open file
        writer = new StreamWriter(logPath, false, Encoding.UTF8);
        writer.AutoFlush = true;

        // Write header first thing
        string headerLine = "frame_index,t_since_start_s,delta_time_s,iso_utc," +
            "Head_ax,Head_ay,Head_az,Hips_ax,Hips_ay,Hips_az,LeftFoot_ax,LeftFoot_ay,LeftFoot_az,RightFoot_ax,RightFoot_ay,RightFoot_az";
        writer.WriteLine(headerLine);
        writer.Flush();

        Debug.Log($"[BodyKinematicsLogger] File created: {logPath}");
        Debug.Log($"[BodyKinematicsLogger] Header: {headerLine}");

        // Initialize timing
        startTime = Time.realtimeSinceStartupAsDouble;
        fixedDelta = 1.0 / sampleRateHz;
        Time.fixedDeltaTime = (float)fixedDelta;
        initialized = true;

        // Initialize positions
        prevHeadPos = head ? head.position : Vector3.zero;
        prevHipsPos = hips ? hips.position : Vector3.zero;
        prevLeftFootPos = leftFoot ? leftFoot.position : Vector3.zero;
        prevRightFootPos = rightFoot ? rightFoot.position : Vector3.zero;
    }

    void FixedUpdate()
    {
        if (!initialized || writer == null) return;

        double tSinceStart = Time.realtimeSinceStartupAsDouble - startTime;
        string isoTime = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
        float dt = (float)fixedDelta;

        // Get current positions
        Vector3 headPos = head ? head.position : Vector3.zero;
        Vector3 hipsPos = hips ? hips.position : Vector3.zero;
        Vector3 leftFootPos = leftFoot ? leftFoot.position : Vector3.zero;
        Vector3 rightFootPos = rightFoot ? rightFoot.position : Vector3.zero;

        // Calculate velocities
        Vector3 headVel = (headPos - prevHeadPos) / dt;
        Vector3 hipsVel = (hipsPos - prevHipsPos) / dt;
        Vector3 leftFootVel = (leftFootPos - prevLeftFootPos) / dt;
        Vector3 rightFootVel = (rightFootPos - prevRightFootPos) / dt;

        // Calculate accelerations
        Vector3 headAcc = (headVel - prevHeadVel) / dt;
        Vector3 hipsAcc = (hipsVel - prevHipsVel) / dt;
        Vector3 leftFootAcc = (leftFootVel - prevLeftFootVel) / dt;
        Vector3 rightFootAcc = (rightFootVel - prevRightFootVel) / dt;

        // Write to file
        string dataLine = $"{frameIndex},{tSinceStart:F6},{dt:F6},{isoTime}," +
            $"{headAcc.x:F6},{headAcc.y:F6},{headAcc.z:F6}," +
            $"{hipsAcc.x:F6},{hipsAcc.y:F6},{hipsAcc.z:F6}," +
            $"{leftFootAcc.x:F6},{leftFootAcc.y:F6},{leftFootAcc.z:F6}," +
            $"{rightFootAcc.x:F6},{rightFootAcc.y:F6},{rightFootAcc.z:F6}";

        writer.WriteLine(dataLine);

        // Update previous states
        prevHeadPos = headPos;
        prevHipsPos = hipsPos;
        prevLeftFootPos = leftFootPos;
        prevRightFootPos = rightFootPos;

        prevHeadVel = headVel;
        prevHipsVel = hipsVel;
        prevLeftFootVel = leftFootVel;
        prevRightFootVel = rightFootVel;

        frameIndex++;

        // Flush periodically
        if (frameIndex % Mathf.Max(1, Mathf.RoundToInt(sampleRateHz)) == 0)
        {
            writer.Flush();
        }
    }

    void OnApplicationPause(bool paused)
    {
        if (paused && writer != null)
        {
            writer.Flush();
            Debug.Log("[BodyKinematicsLogger] Paused - data flushed");
        }
    }

    void OnDestroy()
    {
        if (activeLogger == this) activeLogger = null;

        if (writer != null)
        {
            writer.Flush();
            writer.Close();
            writer.Dispose();
            writer = null;
            Debug.Log($"[BodyKinematicsLogger] Log saved to {logPath}");
        }
    }

    void OnApplicationQuit()
    {
        if (writer != null)
        {
            writer.Flush();
            writer.Close();
            writer.Dispose();
            writer = null;
            Debug.Log($"[BodyKinematicsLogger] Log saved to {logPath}");
        }
    }
}
