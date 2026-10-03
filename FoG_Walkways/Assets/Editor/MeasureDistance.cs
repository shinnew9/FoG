using UnityEditor;
using UnityEngine;

/// <summary>
/// Tools > Measure Distance Between 2 Selected — select exactly two objects
/// (e.g. StartGate + EndGate) and this logs the straight-line and per-axis
/// world distances, so gate spacing can be set precisely without eyeballing.
/// </summary>
public static class MeasureDistance
{
    [MenuItem("Tools/Measure Distance Between 2 Selected")]
    static void Measure()
    {
        var sel = Selection.transforms;
        if (sel.Length != 2)
        {
            Debug.LogWarning($"[Measure] Select exactly 2 objects (currently {sel.Length}).");
            return;
        }

        Vector3 a = sel[0].position, b = sel[1].position;
        Vector3 d = b - a;
        float horizontal = new Vector2(d.x, d.z).magnitude;

        Debug.Log($"[Measure] '{sel[0].name}' ↔ '{sel[1].name}'  " +
                  $"horizontal = {horizontal:F3} m | dX = {Mathf.Abs(d.x):F3}, dY = {Mathf.Abs(d.y):F3}, dZ = {Mathf.Abs(d.z):F3} | 3D = {d.magnitude:F3} m");
    }
}
