using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlashyColorsToggle : MonoBehaviour
{
    public Toggle toggle;
    public void ToggleFlashyColors()
    {
        bool value = toggle.isOn;
        GameManager.Instance.SetTrippyColorsOptionEnabled(value);
        
        PlayerPrefs.SetInt("EnbabledFlashyColors", Convert.ToInt32(value));
        PlayerPrefs.Save();

        //Debug.Log("PLayer changed and saved toggled colors to: " + value);

    }

    private void Awake()
    {
        bool toggleAwakeStatus = GameManager.Instance.GetTrippyColorsOptionEnabled();
        toggle.isOn = toggleAwakeStatus;
    }
}
