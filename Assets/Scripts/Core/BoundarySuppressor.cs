using UnityEngine;

/// <summary>
/// Suppresses the Quest Guardian boundary while this app runs, so FoG
/// participants can walk the full 9.25m without grey passthrough fade
/// or "Create boundary" popups. Replaces the adb
/// `setprop debug.oculus.guardian_pause 1` workaround — no PC needed.
///
/// The OS only grants boundary suppression while a passthrough layer is
/// actively running (safety rule), so this also spins up an invisible
/// passthrough underlay — the opaque VR scene fully covers it.
///
/// Requires (one-time, project-wide, in OVRManager inspector > Quest Features):
///   - Passthrough Support: Supported
///   - Boundary Visibility Support: Supported
/// Attach this to one GameObject in every scene (MainMenu, ClutteredWalkway,
/// NarrowedWalkway).
/// </summary>
public class BoundarySuppressor : MonoBehaviour
{
    void Start()
    {
        var manager = OVRManager.instance;
        if (manager == null)
        {
            Debug.LogWarning("[BoundarySuppressor] No OVRManager in scene — boundary stays on.");
            return;
        }

        manager.isInsightPassthroughEnabled = true;

        // The runtime refuses suppression unless passthrough is actually
        // composited, so add a hidden underlay layer behind the VR scene.
        var layer = manager.GetComponent<OVRPassthroughLayer>();
        if (layer == null)
        {
            layer = manager.gameObject.AddComponent<OVRPassthroughLayer>();
        }
        layer.overlayType = OVROverlay.OverlayType.Underlay;
        layer.compositionDepth = 0;
        layer.hidden = false;
        layer.enabled = true;

        // OVRManager re-sends this request every frame until the OS accepts.
        manager.shouldBoundaryVisibilityBeSuppressed = true;

        Debug.Log("[BoundarySuppressor] Passthrough underlay active, boundary suppression requested.");
    }
}
