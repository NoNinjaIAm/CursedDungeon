using UnityEngine;

public class FullscreenButton : MonoBehaviour
{
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("Fullscreen Toggled");
    }
}
