using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioSceneLoader : MonoBehaviour
{
    [Header("Scene Names")]
    public string mainMenuSceneName = "MainMenu";

    [Header("Debug")]
    public bool debugLogs = true;

    private void Update()
    {
        // Y button only: return to Main Menu
        // OVRInput.Button.Four = Y
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            LoadMainMenu();
        }
    }

    public void LoadMainMenu()
    {
        if (debugLogs)
        {
            Debug.Log("[ScenarioSceneLoader] Loading MainMenu");
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}