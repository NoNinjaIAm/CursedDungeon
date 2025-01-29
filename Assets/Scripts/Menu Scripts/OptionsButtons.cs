using System;
using Unity.VisualScripting;
using UnityEngine;

public class OptionsButtons : MonoBehaviour
{
    // This will invoke true if we are entering menu screen and false if we are exiting
    public static event Action<bool> OnOptionsToggled;

    public void OnOptionButtonClicked()
    {
        SoundManager.Instance.PlaySound("Menu Button Click");
        OnOptionsToggled?.Invoke(true);
    }

    public void OnBackButtonClicked()
    {
        SoundManager.Instance.PlaySound("Menu Button Click");
        OnOptionsToggled?.Invoke(false);
        SoundManager.Instance.SaveSoundSettings();
    }
}
