using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sound Lists")]
    public List<Sound> sfxSounds;
    public List<Sound> themeSounds;

    [Header("Audio Sources")]
    public AudioSource themeSource;   // dedicated source for theme music
    public AudioSource sfxPrefab;     // prefab for pooled SFX sources

    private List<AudioSource> sfxPool = new List<AudioSource>();

    private void Awake()
    {
        instance = this;
    }

    // Play a theme track by name
    public void PlayTheme(string soundName)
    {
        Sound s = themeSounds.Find(x => x.name == soundName);
        if (s != null && themeSource != null)
        {
            themeSource.clip = s.clip;   // assign the clip
            themeSource.loop = true;     // usually theme music loops
            themeSource.Play();
        }
        else
        {
            Debug.LogWarning("Theme sound not found: " + soundName);
        }
    }

    // Play a one-shot SFX by name
    public void PlaySFX(string soundName)
    {
        Sound s = sfxSounds.Find(x => x.name == soundName);
        if (s != null)
        {
            AudioSource source = GetAvailableSource();
            source.PlayOneShot(s.clip);
        }
        else
        {
            Debug.LogWarning("SFX sound not found: " + soundName);
        }
    }

    // Get an available pooled AudioSource for SFX
    private AudioSource GetAvailableSource()
    {
        foreach (var src in sfxPool)
        {
            if (!src.isPlaying)
                return src;
        }

        // If none are free, create a new one
        AudioSource newSource = Instantiate(sfxPrefab, transform);
        sfxPool.Add(newSource);
        return newSource;
    }
}