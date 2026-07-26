using UnityEngine;

public class ManualRedirector : MonoBehaviour
{
    [Header("References")]
    public Transform worldRoot; 
    public Transform head;       // CenterEyeAnchor (HMD)
    public Transform rightController; //  the controller Transform to follow

    [Header("Input")]
    // Hold this to enable world-lock
    public OVRInput.Button holdButton = OVRInput.Button.SecondaryHandTrigger; // Right Grip

    [Header("Options")]
    public bool rotateAroundHeadOnFloor = true; // pivot at feet
    public bool useControllerYaw = true;        // true: follow controller; false: follow head

    float _startYaw;      
    bool _active;

    void Update()
    {
        if (worldRoot == null || head == null) return;

        // Button pressed this frame 
        if (OVRInput.GetDown(holdButton))
        {
            _active = true;
            _startYaw = GetCurrentYaw(); // degrees
        }

        // Button released -> stop
        if (OVRInput.GetUp(holdButton))
        {
            _active = false;
        }

        if (!_active) return;

        // How much we've physically turned since we started holding
        float currentYaw = GetCurrentYaw();
        float deltaYaw = Mathf.DeltaAngle(_startYaw, currentYaw); // (-180..180)

        // Counter-rotate the world so virtual heading stays constant
        RotateWorld(+deltaYaw);

     
        _startYaw = currentYaw;
    }

    float GetCurrentYaw()
    {
        Transform t = useControllerYaw && rightController != null ? rightController : head;
        // Yaw from world up
        return Quaternion.LookRotation(Vector3.ProjectOnPlane(t.forward, Vector3.up), Vector3.up).eulerAngles.y;
    }

    void RotateWorld(float deltaDeg)
    {
        Vector3 pivot = head.position;
        if (rotateAroundHeadOnFloor) pivot.y = worldRoot.position.y;
        worldRoot.RotateAround(pivot, Vector3.up, deltaDeg);
    }
}

