using UnityEditor;
using UnityEngine;

/// <summary>
/// Tools > Print World Position — logs the WORLD-space position and renderer
/// bounds of every selected object. Inspector shows local coordinates for
/// children, which can't be compared across parents; this prints comparable
/// world values.
/// </summary>
public static class PrintWorldPosition
{
    [MenuItem("Tools/Print World Position")]
    static void Print()
    {
        foreach (Transform t in Selection.transforms)
        {
            string msg = $"[WorldPos] '{t.name}' world position = {t.position.ToString("F4")}";
            var r = t.GetComponentInChildren<Renderer>();
            if (r != null)
            {
                msg += $" | bounds top Y = {r.bounds.max.y:F4}, bottom Y = {r.bounds.min.y:F4}";
            }
            Debug.Log(msg);
        }
    }
}
