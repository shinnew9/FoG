using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkwayCalibration : MonoBehaviour{
    [Header("References")]
    [Tooltip("Root object of the Meta Camera Rig, e.g., [BuildingBlock] Camera Rig")]
    public Transform cameraRigRoot;

    [Tooltip("HMD camera transform, e.g., Main Camera or CenterEyeAnchor")]
    public Transform headPoint;

    [Tooltip("Where the participant's head should be after calibration")]
    public Transform targetHeadPoint;

    [Tooltip("A point placed in the intended walking direction")]
    public Transform targetForwardPoint;

    [Header("Input")]
    [Tooltip("Default: X button on left controller")]
    public OVRInput.Button calibrateButton = OVRInput.Button.Three;

    [Tooltip("Useful for testing in Unity Editor")]
    public KeyCode keyboardCalibrateKey = KeyCode.C;

    [Header("Calibration Options")]
    public bool calibrateOnStart = false;
    public bool alignYaw = true;
    public bool alignPositionXZ = true;
    public bool alignHeightY = true;

    [Header("Debug")]
    public bool debugLogs = true;

    private void Start(){
        ConfigureForScene();
        if (calibrateOnStart)
        {
            Calibrate();
        }
    }

    private void ConfigureForScene(){
        string sceneName = SceneManager.GetActiveScene().name;

        alignHeightY = false;
        if (debugLogs) Debug.Log($"[WalkwayCalibration] {sceneName} detected - Height alignment disabled");
    }

    private void Update(){
        bool controllerPressed = OVRInput.GetDown(calibrateButton);

        if (controllerPressed){
            Calibrate();
        }
    }

    public void Calibrate(){
        if (cameraRigRoot == null)
        {
            Debug.LogWarning("[WalkwayCalibration] cameraRigRoot is missing.");
            return;
        }

        if (headPoint == null)
        {
            Debug.LogWarning("[WalkwayCalibration] headPoint is missing.");
            return;
        }

        if (targetHeadPoint == null)
        {
            Debug.LogWarning("[WalkwayCalibration] targetHeadPoint is missing.");
            return;
        }

        if (alignYaw && targetForwardPoint != null)
        {
            AlignYawToWalkway();
        }

        if (alignPositionXZ || alignHeightY)
        {
            AlignPositionToTargetHead();
        }

        if (debugLogs)
        {
            Debug.Log("[WalkwayCalibration] Calibration complete.");
        }
    }

    private void AlignYawToWalkway(){
        Vector3 currentForward = headPoint.forward;
        currentForward.y = 0f;

        Vector3 desiredForward = targetForwardPoint.position - targetHeadPoint.position;
        desiredForward.y = 0f;

        if (currentForward.sqrMagnitude < 0.0001f)
        {
            Debug.LogWarning("[WalkwayCalibration] Current forward vector is too small.");
            return;
        }

        if (desiredForward.sqrMagnitude < 0.0001f)
        {
            Debug.LogWarning("[WalkwayCalibration] Desired forward vector is too small.");
            return;
        }

        float yawAngle = Vector3.SignedAngle(
            currentForward.normalized,
            desiredForward.normalized,
            Vector3.up
        );

        cameraRigRoot.RotateAround(headPoint.position, Vector3.up, yawAngle);

        if (debugLogs)
        {
            Debug.Log($"[WalkwayCalibration] Yaw aligned by {yawAngle:F2} degrees.");
        }
    }

    private void AlignPositionToTargetHead(){
        Vector3 offset = targetHeadPoint.position - headPoint.position;

        if (!alignPositionXZ)
        {
            offset.x = 0f;
            offset.z = 0f;
        }

        if (!alignHeightY)
        {
            offset.y = 0f;
        }

        cameraRigRoot.position += offset;

        if (debugLogs)
        {
            Debug.Log($"[WalkwayCalibration] Position offset applied: {offset}");
        }
    }
}