using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneLoader : MonoBehaviour{
    [Header("Scene Names")]
    public string clutteredSceneName = "ClutteredWalkway";
    public string narrowedSceneName = "NarrowedWalkway";

    [Header("Debug")]
    public bool debugLogs = true;

    public void LoadClutteredWalkway(){
        if (debugLogs)
            Debug.Log("[MainMenu] Loading ClutteredWalkway");

        SceneManager.LoadScene(clutteredSceneName);
    }

    public void LoadNarrowedWalkway(){
        if (debugLogs)
            Debug.Log("[MainMenu] Loading NarrowedWalkway");

        SceneManager.LoadScene(narrowedSceneName);
    }

    void Update(){
        // Right controller A button
        if (OVRInput.GetDown(OVRInput.Button.One)){
            LoadClutteredWalkway();
        }

        // Right controller B button
        if (OVRInput.GetDown(OVRInput.Button.Two)){
            LoadNarrowedWalkway();
        }
    }
}