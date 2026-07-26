using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{

    void Update()
    {
        // Y button (left controller)
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            Debug.Log("[Scene] Returning to menu...");
            SceneManager.LoadScene("ScenarioMenu");
        }
    }
}