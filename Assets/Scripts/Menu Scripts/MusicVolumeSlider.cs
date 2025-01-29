using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        volumeSlider = GetComponent<Slider>();

        // Initialize the slider value to the current volume
        if (volumeSlider != null)
        {
            Debug.Log("Setting Initial Slider with Value of: " + SoundManager.Instance.GetMusicVolume());
            volumeSlider.value = SoundManager.Instance.GetMusicVolume();
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    private void OnEnable()
    {
        
    }

    // Set the volume based on the slider value
    public void SetVolume(float volume)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ChangeMusicVolume(volume);
        }
        else
        {
            Debug.LogError("No MusicSrc Component to Play From Detected");
        }
    }
}
