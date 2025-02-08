using UnityEngine;

public class FlashyColorsToggle : MonoBehaviour
{
    public void ToggleFlashyColors()
    {
        GameManager.Instance.SetTrippyColorsOptionEnabled(!GameManager.Instance.GetTrippyColorsOptionEnabled());
    }
}
