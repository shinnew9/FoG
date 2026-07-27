using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonLoader : MonoBehaviour
{
    public void Load6mAnd3m()
    {
        SceneManager.LoadScene("Freeze_of_Gait_HighNarrowWalkway");
    }

    public void LoadClosedDoor()
    {
        SceneManager.LoadScene("Freeze_of_Gait_Walkway_with_clutter");
    }
}