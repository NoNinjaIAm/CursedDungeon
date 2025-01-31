using System;
using UnityEngine;

public class HowToPlayButtons : MonoBehaviour
{
    // This will invoke true if we are entering menu screen and false if we are exiting
    public static event Action<bool> OnHowToPlayToggled;

    public void OnHowToButtonClicked()
    {
        SoundManager.Instance.PlaySound("Menu Button Click");
        OnHowToPlayToggled?.Invoke(true);
    }

    public void OnBackButtonClicked()
    {
        SoundManager.Instance.PlaySound("Menu Button Click");
        OnHowToPlayToggled?.Invoke(false);
        SoundManager.Instance.SaveSoundSettings();
    }
}
