using UnityEngine;

public class StartCalibration : MonoBehaviour
{
    [Header("Player / World")]
    public Transform headPoint;   // CenterEyeAnchor
    public Transform worldRoot;

    [Header("6m Scenario")]
    public Transform startPoint6m;
    public Transform forwardPoint6m;

    [Header("3m Scenario")]
    public Transform startPoint3m;
    public Transform forwardPoint3m;

    [Header("Buttons")]
    public OVRInput.Button select6mButton = OVRInput.Button.One;     // A
    public OVRInput.Button select3mButton = OVRInput.Button.Two;     // B
    public OVRInput.Button calibrateButton = OVRInput.Button.Three;  // X
    public OVRInput.Button manualAdjustGripButton = OVRInput.Button.SecondaryHandTrigger;  // Right Grip
    public OVRInput.Axis2D manualAdjustAxis = OVRInput.Axis2D.SecondaryThumbstick;

    [Header("Body Alignment")]
    public float floorY = 0f;
    public Vector3 headToBodyOffset = Vector3.zero;

    [Header("Manual Adjustment")]
    public float adjustmentSpeed = 0.5f;  // m/s
    public bool enableManualAdjust = true;

    [Header("Debug")]
    public bool debugLogs = true;

    private Transform activeStartPoint;
    private Transform activeForwardPoint;

    private Vector3 realStartPoint;
    private Vector3 realForwardPoint;
    private bool firstPointCaptured = false;
    private bool manualAdjustMode = false;

    void Start()
    {
        // Default to 6m
        activeStartPoint = startPoint6m;
        activeForwardPoint = forwardPoint6m;

        if (debugLogs)
            Debug.Log("[Calibration] Default scenario = 6m");
    }

    void Update()
    {
        if (OVRInput.GetDown(select6mButton))
        {
            activeStartPoint = startPoint6m;
            activeForwardPoint = forwardPoint6m;
            firstPointCaptured = false;
            manualAdjustMode = false;

            if (debugLogs)
                Debug.Log("[Calibration] Selected 6m scenario");
        }

        if (OVRInput.GetDown(select3mButton))
        {
            activeStartPoint = startPoint3m;
            activeForwardPoint = forwardPoint3m;
            firstPointCaptured = false;
            manualAdjustMode = false;

            if (debugLogs)
                Debug.Log("[Calibration] Selected 3m scenario");
        }

        // Manual adjustment mode: X + Right Grip + Thumbstick
        if (enableManualAdjust && OVRInput.Get(calibrateButton) && OVRInput.Get(manualAdjustGripButton))
        {
            if (!manualAdjustMode)
            {
                manualAdjustMode = true;
                if (debugLogs)
                    Debug.Log("[Calibration] Manual adjustment mode ON");
            }

            Vector2 thumbstick = OVRInput.Get(manualAdjustAxis);
            if (thumbstick.sqrMagnitude > 0.1f)
            {
                Vector3 moveDir = new Vector3(thumbstick.x, 0f, thumbstick.y);
                worldRoot.position += moveDir * adjustmentSpeed * Time.deltaTime;

                if (debugLogs)
                    Debug.Log($"[Calibration] Manual adjust: {moveDir.normalized}, pos: {worldRoot.position}");
            }
        }
        else if (manualAdjustMode)
        {
            manualAdjustMode = false;
            if (debugLogs)
                Debug.Log("[Calibration] Manual adjustment mode OFF");
        }

        // Auto calibrate (only if NOT in manual adjust mode)
        if (!manualAdjustMode && OVRInput.GetDown(calibrateButton))
        {
            if (debugLogs)
                Debug.Log("[Calibration] Calibrate button pressed.");

            if (!firstPointCaptured)
                CaptureFirstPoint();
            else
                CaptureSecondPointAndCalibrate();
        }
    }

    Vector3 GetBodyGroundPoint()
    {
        Vector3 p = headPoint.position;
        p.y = floorY;
        p += headToBodyOffset;
        return p;
    }

    void CaptureFirstPoint()
    {
        realStartPoint = GetBodyGroundPoint();
        firstPointCaptured = true;

        if (debugLogs)
            Debug.Log("[Calibration] First point captured: " + realStartPoint);
    }

    void CaptureSecondPointAndCalibrate()
    {
        if (activeStartPoint == null || activeForwardPoint == null || worldRoot == null)
        {
            Debug.LogError("[Calibration] Missing active scenario references.");
            return;
        }

        realForwardPoint = GetBodyGroundPoint();

        Vector3 realDirection = realForwardPoint - realStartPoint;
        realDirection.y = 0f;

        if (realDirection.sqrMagnitude < 0.0001f)
        {
            Debug.LogWarning("[Calibration] Second point too close to first point.");
            firstPointCaptured = false;
            return;
        }
        realDirection.Normalize();

        Vector3 virtualStart = activeStartPoint.position;
        virtualStart.y = floorY;

        Vector3 virtualForward = activeForwardPoint.position;
        virtualForward.y = floorY;

        Vector3 virtualDirection = virtualForward - virtualStart;

        if (virtualDirection.sqrMagnitude < 0.0001f)
        {
            Debug.LogWarning("[Calibration] Virtual forward point too close to start point.");
            firstPointCaptured = false;
            return;
        }
        virtualDirection.Normalize();

        float signedAngle = Vector3.SignedAngle(virtualDirection, realDirection, Vector3.up);
        worldRoot.RotateAround(activeStartPoint.position, Vector3.up, signedAngle);

        Vector3 rotatedVirtualStart = activeStartPoint.position;
        rotatedVirtualStart.y = floorY;

        Vector3 translation = realStartPoint - rotatedVirtualStart;
        worldRoot.position += translation;

        if (debugLogs)
        {
            Debug.Log("[Calibration] Calibration complete.");
            Debug.Log("[Calibration] Signed angle: " + signedAngle);
            Debug.Log("[Calibration] Translation: " + translation);
        }

        firstPointCaptured = false;
    }
}