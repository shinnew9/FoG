using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using UnityEngine;
using System.Text;

public class BodyKinematicsLogger : MonoBehaviour
{
    public double StartTimeRealtime { get { return startTime; } }


    [Header("Logging Settings")]
    public Transform hips;
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform head;
    public float sampleRateHz = 15f; // how often to log, in Hz

    private string logPath;
    private StreamWriter writer;
    private double startTime;
    private double fixedDelta;
    private int frameIndex = 0;
    private bool initialized = false;

    // Previous positions and velocities for each body part
    private Vector3 prevHeadPos, prevHipsPos, prevLeftFootPos, prevRightFootPos;
    private Vector3 prevHeadVel, prevHipsVel, prevLeftFootVel, prevRightFootVel;

    void Start()
    {
        // Setup folder
        string folder = Path.Combine(Application.persistentDataPath, "_LocalLogs");
        Directory.CreateDirectory(folder);

        string fileName = $"BodyTracking_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
        logPath = Path.Combine(folder, fileName);

        // Open file and write header
        writer = new StreamWriter(logPath, false);
        writer.WriteLine("frame_index,t_since_start_s,delta_time_s,iso_utc," +
            "Head_ax,Head_ay,Head_az,Hips_ax,Hips_ay,Hips_az,LeftFoot_ax,LeftFoot_ay,LeftFoot_az,RightFoot_ax,RightFoot_ay,RightFoot_az");
        writer.Flush();

        // Record reference time
        startTime = Time.realtimeSinceStartupAsDouble;
        fixedDelta = 1.0 / sampleRateHz;
        Time.fixedDeltaTime = (float)fixedDelta;
        initialized = true;

        // Initialize previous positions
        prevHeadPos = head ? head.position : Vector3.zero;
        prevHipsPos = hips ? hips.position : Vector3.zero;
        prevLeftFootPos = leftFoot ? leftFoot.position : Vector3.zero;
        prevRightFootPos = rightFoot ? rightFoot.position : Vector3.zero;

        Debug.Log($"[BodyTrackingLogger] Logging to: {logPath}");
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

        // Compute current velocities
        Vector3 headVel = (headPos - prevHeadPos) / dt;
        Vector3 hipsVel = (hipsPos - prevHipsPos) / dt;
        Vector3 leftFootVel = (leftFootPos - prevLeftFootPos) / dt;
        Vector3 rightFootVel = (rightFootPos - prevRightFootPos) / dt;

        // Compute accelerations
        Vector3 headAcc = (headVel - prevHeadVel) / dt;
        Vector3 hipsAcc = (hipsVel - prevHipsVel) / dt;
        Vector3 leftFootAcc = (leftFootVel - prevLeftFootVel) / dt;
        Vector3 rightFootAcc = (rightFootVel - prevRightFootVel) / dt;

        // Write to file
        writer.WriteLine($"{frameIndex},{tSinceStart:F6},{dt:F6},{isoTime}," +
            $"{headAcc.x:F6},{headAcc.y:F6},{headAcc.z:F6}," +
            $"{hipsAcc.x:F6},{hipsAcc.y:F6},{hipsAcc.z:F6}," +
            $"{leftFootAcc.x:F6},{leftFootAcc.y:F6},{leftFootAcc.z:F6}," +
            $"{rightFootAcc.x:F6},{rightFootAcc.y:F6},{rightFootAcc.z:F6}");

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
    }

    void OnApplicationQuit()
    {
        if (writer != null)
        {
            writer.Flush();
            writer.Close();
            Debug.Log($"[BodyTrackingLogger] Log saved to {logPath}");
        }
    }
}
