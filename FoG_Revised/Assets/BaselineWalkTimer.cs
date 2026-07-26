/*using UnityEngine;

public class BaselineWalkTimer : MonoBehaviour
{
   
    [Header("References")]
    public Transform playerPoint;   // drag PlayerCollider here
    public Transform startLine;     // drag StartLine here
    public Transform stopLine;      // drag StopLine here

    [Header("Debug")]
    public bool debugLogs = true;

    private bool started = false;
    private bool finished = false;
    private float startTime;

    private float prevStartSide;
    private float prevStopSide;

    void Start()
    {
        if (playerPoint == null || startLine == null || stopLine == null)
        {
            Debug.LogError("[Baseline] Missing references in inspector.");
            enabled = false;
            return;
        }

        // Record which side of each line the player starts on
        prevStartSide = SideValue(startLine, playerPoint.position);
        prevStopSide = SideValue(stopLine, playerPoint.position);

        if (debugLogs) Debug.Log("[Baseline] Ready. Waiting to cross StartLine.");
    }

    void Update()
    {
        if (finished) return;

        if (!started)
        {
            float curStartSide = SideValue(startLine, playerPoint.position);

            // If sign changes, player crossed the line
            if (Mathf.Sign(curStartSide) != Mathf.Sign(prevStartSide))
            {
                started = true;
                startTime = Time.time;
                if (debugLogs) Debug.Log("[Baseline] START timing");
            }

            prevStartSide = curStartSide;
            return;
        }

        float curStopSide = SideValue(stopLine, playerPoint.position);

        if (Mathf.Sign(curStopSide) != Mathf.Sign(prevStopSide))
        {
            finished = true;
            float duration = Time.time - startTime;
            if (debugLogs) Debug.Log($"[Baseline] STOP timing. Duration = {duration:F3}s");
        }

        prevStopSide = curStopSide;
    }

    // Returns + or - depending on which side of the line you're on.
    private float SideValue(Transform line, Vector3 point)
    {
        return Vector3.Dot(line.forward, point - line.position);
    }

    public void ResetTrial()
    {
        started = false;
        finished = false;

        prevStartSide = SideValue(startLine, playerPoint.position);
        prevStopSide = SideValue(stopLine, playerPoint.position);

        if (debugLogs) Debug.Log("[Baseline] RESET");
    }
}
*/