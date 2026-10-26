using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioUIController : MonoBehaviour
{
    [Header("References")]
    public AudioManager audioManager;

    [Header("UI Elements")]
    public Button playButton;
    public Button pauseButton;
    public Button stopButton;
    public Button nextButton;
    public Button sfxTestButton;

    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider ambientSlider;

    public TMP_Text currentTrackText;
    public TMP_Text timeText;

    private bool isPlaying = false;

    void Start()
    {
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayButtonClicked);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseButtonClicked);

        if (stopButton != null)
            stopButton.onClick.AddListener(OnStopButtonClicked);

        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextButtonClicked);

        if (sfxTestButton != null)
            sfxTestButton.onClick.AddListener(OnSFXTestButtonClicked);

        if (musicSlider != null)
        {
            musicSlider.value = audioManager.musicVolume;
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = audioManager.sfxVolume;
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        if (ambientSlider != null)
        {
            ambientSlider.value = audioManager.ambientVolume;
            ambientSlider.onValueChanged.AddListener(OnAmbientVolumeChanged);
        }
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (currentTrackText != null && audioManager.backgroundMusic.Count > 0)
        {
            int currentIndex = audioManager.currentMusicIndex;
            string trackName = audioManager.backgroundMusic[currentIndex].name;
            currentTrackText.text = $"Сейчас играет: {trackName}";
        }

        if (timeText != null && audioManager.musicSource != null && audioManager.musicSource.isPlaying)
        {
            float currentTime = audioManager.musicSource.time; 
            float totalTime = audioManager.musicSource.clip.length; 

            string currentFormatted = string.Format("{0:00}:{1:00}", Mathf.Floor(currentTime / 60), Mathf.Floor(currentTime % 60));
            string totalFormatted = string.Format("{0:00}:{1:00}", Mathf.Floor(totalTime / 60), Mathf.Floor(totalTime % 60));

            timeText.text = $"Time: {currentFormatted} / {totalFormatted}";
        }
        else if (timeText != null && !audioManager.musicSource.isPlaying)
        {
            timeText.text = "Time: 00:00";
        }

        if (playButton != null)
            playButton.interactable = !isPlaying;

        if (pauseButton != null)
            pauseButton.interactable = isPlaying;
    }

    public void OnPlayButtonClicked()
    {
        audioManager.PlayBackgroundMusic();
        isPlaying = true;
    }

    public void OnPauseButtonClicked()
    {
        audioManager.PauseMusic();
        isPlaying = false;
    }

    public void OnStopButtonClicked()
    {
        audioManager.StopMusic();
        isPlaying = false;
    }

    public void OnNextButtonClicked()
    {
        audioManager.SkipToNextTrack();
    }

    public void OnSFXTestButtonClicked()
    {
        if (audioManager.soundEffects.Count > 0)
        {
            int randomIndex = Random.Range(0, audioManager.soundEffects.Count);
            audioManager.PlaySFX(audioManager.soundEffects[randomIndex]);
        }
    }

    public void OnMusicVolumeChanged(float value)
    {
        audioManager.SetMusicVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        audioManager.SetSFXVolume(value);
    }

    public void OnAmbientVolumeChanged(float value)
    {
        audioManager.SetAmbientVolume(value);
    }
}
