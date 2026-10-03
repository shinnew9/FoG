using UnityEngine;
using TMPro;
using XRMultiplayer;

public class PatientIDInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI patientIDDisplay;
    private string currentPatientID = "";

    void Start()
    {
        if (patientIDDisplay == null)
        {
            Debug.LogWarning("[PatientIDInput] PatientIDDisplay TextMeshProUGUI not assigned!");
        }
        else
        {
            UpdateDisplay();
        }
    }

    public void OnInputField(string input)
    {
        currentPatientID = input;
        UpdateDisplay();
    }

    public void ConfirmPatientID()
    {
        if (string.IsNullOrEmpty(currentPatientID))
        {
            Debug.LogWarning("[PatientIDInput] Patient ID is empty!");
            return;
        }

        SessionDataManager.Instance.SetPatientID(currentPatientID);
        Debug.Log($"[PatientIDInput] Patient ID confirmed: {currentPatientID}");
    }

    private void UpdateDisplay()
    {
        if (patientIDDisplay != null)
        {
            patientIDDisplay.text = $"Patient ID: {(string.IsNullOrEmpty(currentPatientID) ? "Not Set" : currentPatientID)}";
        }
    }

    public string GetPatientID()
    {
        return currentPatientID;
    }
}
