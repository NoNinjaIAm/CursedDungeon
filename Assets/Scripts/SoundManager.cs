using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] sfxs;
    private float musicVolume;
    public AudioSource musicSource;

    private float defaultMusicVolume = 0.117f;

    // This is a lazy singleton instance that can be used universally to play sounds!!
    // Architecture is the music has one continous audio source attached to the gameObject
    // The sounds create a temp audio source on top of music one to play sounds so sounds can overlap
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // This script will create a SoundManager from the Resources folder

    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Load the prefab dynamically if it doesn't exist
                GameObject soundManagerPrefab = Resources.Load<GameObject>("SoundManager");
                if (soundManagerPrefab != null)
                {
                    GameObject soundManager = Instantiate(soundManagerPrefab);
                    instance = soundManager.GetComponent<SoundManager>();
                }
                else
                {
                    Debug.LogError("SoundManager prefab not found in Resources folder!");
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSoundSettings();
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Avoid duplicate sound managers
        }
    }

    public void PlaySound(string soundName)
    {
        AudioClip sound = GetSoundClipByName(soundName);
        if (sound != null)
        {
            AudioSource tempSrc = gameObject.AddComponent<AudioSource>();
            tempSrc.volume = 0.1f;
            tempSrc.PlayOneShot(sound);
            Destroy(tempSrc, sound.length);
        }
        else
        {
            Debug.LogWarning("SOUND: " + soundName + " NOT FOUND");
        }
    }
    public void PlayMusic(string musicClipName)
    {
        AudioClip musicClip = GetSoundClipByName(musicClipName);
        if (musicClip != null)
        {
            ChangeMusicVolume(musicVolume); // Make sure music volume is updated
            musicSource.clip = musicClip;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("MUSIC CLIP: " + musicClip + " NOT FOUND");
        }
        
    }

    public void ChangeMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicVolume = volume;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SaveSoundSettings()
    {
        // Save highsore
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    private void LoadSoundSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    private AudioClip GetSoundClipByName(string name)
    {
        foreach (AudioClip clip in sfxs)
        {
            if (clip.name == name) return clip;
        }
        return null;
    }
}
