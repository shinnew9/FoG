using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonLoader : MonoBehaviour
{
    public void Load6mAnd3m()
    {
        SceneManager.LoadScene("Freeze_of_Gait 1");
    }

    public void LoadClosedDoor()
    {
        SceneManager.LoadScene("Freeze_of_Gait_Closed_Door");
    }

    public void LoadDoorway()
    {
        SceneManager.LoadScene("Freeze_of_Gait_Doorway");
    }
}