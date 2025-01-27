using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] sfxs;
    public AudioClip music;
    public AudioSource musicSource;

    public static SoundManager instance;

    public float musicVolume;


    // This is a singleton instance that can be used universally to play sounds!!
    // Architecture is the music has one continous audio source attached to the gameObject
    // The sounds create a temp audio source on top of music one to play sounds so sounds can overlap
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction during scene transitions
        }
        else
        {
            Destroy(gameObject); // Avoid duplicate music players
        }
        musicSource.clip = music;
        musicVolume = 1.0f;
    }

    public void PlaySound(string soundName)
    {
        AudioClip sound = GetSoundClipByName(soundName);
        if (sound != null)
        {
            AudioSource tempSrc = gameObject.AddComponent<AudioSource>();
            tempSrc.PlayOneShot(sound);
            Destroy(tempSrc, sound.length);
        }
        else
        {
            Debug.LogError("SOUND: " + soundName + " NOT FOUND");
        }
    }
    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }

    public void StopMusic()
    {
        musicSource.Stop();
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
