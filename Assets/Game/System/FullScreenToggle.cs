using UnityEngine;

public sealed class FullScreenToggle : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            ToggleFullScreen();
        }
    }
    
    void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
