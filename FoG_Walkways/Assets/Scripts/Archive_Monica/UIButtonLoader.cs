using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using XRMultiplayer;

public class UIButtonLoader : MonoBehaviour
{
    private void ResetCameraRig()
    {
        XROrigin xrOrigin = FindFirstObjectByType<XROrigin>();
        if (xrOrigin != null)
        {
            xrOrigin.transform.position = Vector3.zero;
            xrOrigin.transform.rotation = Quaternion.identity;
            Debug.Log("[UIButtonLoader] Camera Rig reset!");
        }
    }

    public void Load6mAnd3m()
    {
        ResetCameraRig();
        SceneManager.LoadScene("Freeze_of_Gait_HighNarrowWalkway");
    }

    public void LoadClosedDoor()
    {
        ResetCameraRig();
        SceneManager.LoadScene("Freeze_of_Gait_Walkway_with_clutter");
    }
}