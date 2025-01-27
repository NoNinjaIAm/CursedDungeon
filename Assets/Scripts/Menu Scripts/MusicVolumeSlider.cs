using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeSlider = GetComponent<Slider>();

        // Initialize the slider value to the current volume
        if (SoundManager.instance != null && volumeSlider != null)
        {
            //Debug.Log("Setting Initial Slider with Value of: " + SoundManager.instance.musicVolume);
            volumeSlider.value = SoundManager.instance.musicVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // Set the volume based on the slider value
    public void SetVolume(float volume)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.ChangeMusicVolume(volume);
        }
        else
        {
            Debug.LogError("No MusicSrc Component to Play From Detected");
        }
    }
}
