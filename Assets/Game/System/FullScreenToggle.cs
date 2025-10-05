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
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
            Screen.SetResolution(640, 480, FullScreenMode.Windowed);
        }
        else
        {
            Screen.fullScreen = true;
            var resolutions = Screen.resolutions;
            var maxResolution = resolutions[^1];
            Screen.SetResolution(maxResolution.height / 3 * 4, maxResolution.height, FullScreenMode.FullScreenWindow);
        }
    }
}
