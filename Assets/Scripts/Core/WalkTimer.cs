using UnityEngine;

public class WalkTimer : MonoBehaviour{
    [Header("Timer State")]
    public bool isTiming = false;
    public bool completed = false;

    [Header("Result")]
    public float startTime;
    public float endTime;
    public float elapsedTime;

    [Header("Optional Logging")]
    public CSVLogger csvLogger;

    [Header("Debug")]
    public bool debugLogs = true;

    public void StartTimer(){
        if (isTiming)
            return;

        completed = false;
        isTiming = true;
        startTime = Time.time;
        elapsedTime = 0f;

        if (debugLogs)
            Debug.Log("[WalkTimer] Timer started.");
    }

    public void StopTimer(){
        if (!isTiming)
            return;

        endTime = Time.time;
        elapsedTime = endTime - startTime;
        isTiming = false;
        completed = true;

        if (debugLogs)
            Debug.Log($"[WalkTimer] Timer stopped. Elapsed time: {elapsedTime:F2} seconds.");

        if (csvLogger != null)
        {
            csvLogger.LogWalkResult(elapsedTime);
        }
    }

    public void ResetTimer(){
        isTiming = false;
        completed = false;
        startTime = 0f;
        endTime = 0f;
        elapsedTime = 0f;

        if (debugLogs)
            Debug.Log("[WalkTimer] Timer reset.");
    }

    private void Update(){
        if (isTiming)
        {
            elapsedTime = Time.time - startTime;
        }
    }
}