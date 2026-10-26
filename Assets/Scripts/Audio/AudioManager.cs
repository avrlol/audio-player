using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;

    [Header("Audio Clips")]
    public List<AudioClip> backgroundMusic;
    public List<AudioClip> soundEffects;
    public List<AudioClip> ambientSounds;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f;
    [Range(0f, 1f)]
    public float ambientVolume = 0.5f;

    public int currentMusicIndex = 0;
    private bool isMusicPlaying = false;
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    void Start()
    {
        InitializeSFXDictionary();
        UpdateVolumes();
        PlayBackgroundMusic();
    }

    void InitializeSFXDictionary()
    {
        foreach (AudioClip clip in soundEffects)
        {
            if (clip != null)
            {
                sfxDictionary[clip.name] = clip;
            }
        }
    }

    void UpdateVolumes()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;

        if (ambientSource != null)
            ambientSource.volume = ambientVolume;
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic.Count == 0 || musicSource == null) return;

        StartCoroutine(PlayMusicSequence());
    }

    private IEnumerator PlayMusicSequence()
    {
        isMusicPlaying = true;

        while (isMusicPlaying)
        {
            musicSource.clip = backgroundMusic[currentMusicIndex];
            musicSource.Play();

            yield return new WaitForSeconds(musicSource.clip.length);

            currentMusicIndex = (currentMusicIndex + 1) % backgroundMusic.Count;
        }
    }

    public void PlaySFX(string clipName)
    {
        if (sfxDictionary.ContainsKey(clipName) && sfxSource != null)
        {
            sfxSource.PlayOneShot(sfxDictionary[clipName]);
        }
        else
        {
            Debug.LogWarning($"SFX not found: {clipName}");
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayAmbient(string clipName)
    {
        if (ambientSource != null)
        {
            foreach (AudioClip clip in ambientSounds)
            {
                if (clip.name == clipName)
                {
                    ambientSource.clip = clip;
                    ambientSource.loop = true;
                    ambientSource.Play();
                    return;
                }
            }
        }
    }

    public void StopMusic()
    {
        isMusicPlaying = false;
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetAmbientVolume(float volume)
    {
        ambientVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SkipToNextTrack()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            currentMusicIndex = (currentMusicIndex + 1) % backgroundMusic.Count;
            if (isMusicPlaying)
            {
                StartCoroutine(PlayMusicSequence());
            }
        }
    }
}
