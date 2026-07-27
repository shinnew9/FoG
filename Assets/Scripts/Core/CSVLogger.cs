using System;
using System.IO;
using UnityEngine;

public class CSVLogger : MonoBehaviour{
    [Header("File Settings")]
    public string fileName = "walkway_results.csv";

    [Header("Experiment Info")]
    public string participantId = "P001";
    public string conditionName = "ClutteredWalkway";
    public int trialNumber = 1;

    [Header("Debug")]
    public bool debugLogs = true;

    private string filePath;

    private void Awake(){
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            string header = "timestamp,participant_id,condition,trial,elapsed_seconds\n";
            File.WriteAllText(filePath, header);

            if (debugLogs)
            {
                Debug.Log($"[CSVLogger] Created CSV file at: {filePath}");
            }
        }
    }

    public void LogWalkResult(float elapsedSeconds){
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string line = $"{timestamp},{participantId},{conditionName},{trialNumber},{elapsedSeconds:F3}\n";

        File.AppendAllText(filePath, line);

        if (debugLogs)
        {
            Debug.Log($"[CSVLogger] Logged result: {line}");
            Debug.Log($"[CSVLogger] File path: {filePath}");
        }
    }
}