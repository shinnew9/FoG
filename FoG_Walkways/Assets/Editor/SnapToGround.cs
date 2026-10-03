using UnityEditor;
using UnityEngine;

/// <summary>
/// Tools > Snap Selected To Ground (Y=0)
/// Moves each selected root object vertically so the lowest point of all
/// its child renderers sits exactly on the world ground plane (Y = 0).
/// Select the PARENT object (e.g. Structure_02(1)) — children follow.
/// </summary>
public static class SnapToGround
{
    [MenuItem("Tools/Snap Selected To Ground (Y=0)")]
    static void Snap()
    {
        foreach (Transform t in Selection.transforms)
        {
            var renderers = t.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
            {
                Debug.LogWarning($"[SnapToGround] '{t.name}' has no renderers — skipped.");
                continue;
            }

            Bounds b = renderers[0].bounds;
            foreach (var r in renderers) b.Encapsulate(r.bounds);

            float offsetY = -b.min.y; // how far the lowest point is below/above Y=0
            Undo.RecordObject(t, "Snap To Ground");
            t.position += new Vector3(0f, offsetY, 0f);

            Debug.Log($"[SnapToGround] '{t.name}' moved by {offsetY:F4} m — lowest point now at Y=0.");
        }
    }
}
