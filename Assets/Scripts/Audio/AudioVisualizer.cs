using UnityEngine;
using UnityEngine.UI;

public class AudioVisualizer : MonoBehaviour
{
    [Header("Visualizer Settings")]
    public int sampleSize = 512;
    public float visualizerScale = 100f;
    public float updateSpeed = 0.1f;

    [Header("UI Elements")]
    public RectTransform visualizerBarPrefab;
    public Transform visualizerContainer;

    private AudioSource audioSource;
    private float[] samples;
    private RectTransform[] visualizerBars;
    private float timer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        samples = new float[sampleSize];

        CreateVisualizerBars();
    }

    void CreateVisualizerBars()
    {
        if (visualizerBarPrefab == null || visualizerContainer == null) return;

        visualizerBars = new RectTransform[sampleSize / 8];

        for (int i = 0; i < visualizerBars.Length; i++)
        {
            visualizerBars[i] = Instantiate(visualizerBarPrefab, visualizerContainer);
            visualizerBars[i].anchoredPosition = new Vector2(i * 10f, 0);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateSpeed && audioSource.isPlaying)
        {
            UpdateVisualizer();
            timer = 0f;
        }
    }

    void UpdateVisualizer()
    {
        if (audioSource == null || visualizerBars == null) return;

        audioSource.GetOutputData(samples, 0);

        for (int i = 0; i < visualizerBars.Length; i++)
        {
            if (visualizerBars[i] != null)
            {
                float sample = Mathf.Abs(samples[i * 8]);
                float newHeight = sample * visualizerScale;

                visualizerBars[i].sizeDelta = new Vector2(8f, newHeight);

                Image barImage = visualizerBars[i].GetComponent<Image>();
                if (barImage != null)
                {
                    barImage.color = Color.Lerp(Color.green, Color.red, sample);
                }
            }
        }
    }
}
