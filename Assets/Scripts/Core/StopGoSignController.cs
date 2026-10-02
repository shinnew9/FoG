using UnityEngine;
using TMPro;

public class StopGoSignController : MonoBehaviour{
    [Header("Text Reference")]
    public TextMeshPro signText;

    [Header("Timing")]
    public float stopDuration = 3f;

    [Header("Display Text")]
    public string stopText = "STOP";
    public string goText = "GO";

    [Header("Debug")]
    public bool debugLogs = true;

    private bool hasStarted = false;

    private void Start(){
        ShowStop();
        Invoke(nameof(ShowGo), stopDuration);
    }

    public void ShowStop(){
        if (signText != null){
            signText.text = stopText;
        }

        hasStarted = false;

        if (debugLogs){
            Debug.Log("[StopGoSignController] STOP shown.");
        }
    }

    public void ShowGo(){
        if (signText != null){
            signText.text = goText;
        }

        hasStarted = true;

        if (debugLogs){
            Debug.Log("[StopGoSignController] GO shown.");
        }
    }

    public bool IsGo(){
        return hasStarted;
    }
}