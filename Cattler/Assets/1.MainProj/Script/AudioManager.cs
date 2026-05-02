using System.Collections.Generic;
using System.Collections;
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
        if (sfxPrefab == null)
        {
            Debug.LogWarning("Theme AudioSource is disabled");
            return; //safety check 
        }

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

    public void TransitionTheme(string soundName)
    {
        Sound s = themeSounds.Find(x => x.name == soundName);
        if (s != null && themeSource != null)
        {
            StartCoroutine(TransitionSequence(s.clip));
        }
    }

    IEnumerator TransitionSequence(AudioClip newClip)
    {
        float originalVolume = themeSource.volume;
        float duration = 1.0f; // fade time in seconds
        float t = 0f;

        // Fade out
        while (t < duration)
        {
            t += Time.deltaTime;
            themeSource.volume = Mathf.Lerp(originalVolume, 0f, t / duration);
            yield return null;
        }

        // Swap clip
        themeSource.clip = newClip;
        themeSource.loop = true;
        themeSource.Play();

        // Fade in
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            themeSource.volume = Mathf.Lerp(0f, originalVolume, t / duration);
            yield return null;
        }

        // Ensure exact reset
        themeSource.volume = originalVolume;
    }


    // Play a one-shot SFX by name
    public void PlaySFX(string soundName)
    {
        if(sfxPrefab == null)
        {
            Debug.LogWarning("SFX AudioSource is disabled");
            return; //safety check 
        }

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

    public void PlaySFX(AudioClip AudioClip)
    {
        if (sfxPrefab == null)
        {
            Debug.LogWarning("SFX AudioSource is disabled");
            return; //safety check 
        }

        if (AudioClip != null)
        {
            AudioSource source = GetAvailableSource();
            source.PlayOneShot(AudioClip);
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