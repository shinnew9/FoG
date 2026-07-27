using UnityEngine;

public class WalkTimerGate : MonoBehaviour{
    public enum GateType{
        Start,
        End
    }

    [Header("Gate Settings")]
    public GateType gateType;
    public WalkTimer walkTimer;

    [Header("Optional Stop-Go Control")]
    public StopGoSignController stopGoController;
    public bool requireGoSignalForStart = false;

    [Header("Trigger Target")]
    public string targetTag = "Player";

    [Header("Debug")]
    public bool debugLogs = true;

    private void OnTriggerEnter(Collider other){
        if (!other.CompareTag(targetTag))
            return;

        if (walkTimer == null){
            Debug.LogWarning("[WalkTimerGate] WalkTimer reference is missing.");
            return;
        }

        if (gateType == GateType.Start){
            if (requireGoSignalForStart && stopGoController != null && !stopGoController.IsGo()){
                if (debugLogs)
                    Debug.Log("[WalkTimerGate] Start gate entered, but GO signal has not appeared yet.");

                return;
            }

            if (debugLogs)
                Debug.Log("[WalkTimerGate] Start gate entered.");

            walkTimer.StartTimer();
        }
        else if (gateType == GateType.End){
            if (debugLogs)
                Debug.Log("[WalkTimerGate] End gate entered.");

            walkTimer.StopTimer();
        }
    }
}