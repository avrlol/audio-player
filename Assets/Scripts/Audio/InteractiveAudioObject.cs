using UnityEngine;

public class InteractiveAudioObject : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip interactionSound;
    public AudioClip hoverSound;
    [Range(0f, 1f)]
    public float volume = 1f;

    [Header("Visual Feedback")]
    public Color hoverColor = Color.yellow;
    public Color clickColor = Color.red;

    private AudioSource audioSource;
    private Material originalMaterial;
    private Material objectMaterial;
    private Color originalColor;

    private AudioManager audioManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioManager = FindAnyObjectByType<AudioManager>();

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            objectMaterial = renderer.material;
            originalMaterial = objectMaterial;
            originalColor = objectMaterial.color;
        }
    }

    void Update()
    {
        if (audioManager != null)
        {
            audioSource.volume = volume * audioManager.sfxVolume;
        }
        else
        {
            audioSource.volume = volume;
        }
    }

    void OnMouseEnter()
    {
        if (objectMaterial != null)
            objectMaterial.color = hoverColor;

        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    void OnMouseExit()
    {
        if (objectMaterial != null)
            objectMaterial.color = originalColor;
    }

    void OnMouseDown()
    {
        if (objectMaterial != null)
            objectMaterial.color = clickColor;

        if (interactionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(interactionSound);
        }
    }

    void OnMouseUp()
    {
        if (objectMaterial != null)
            objectMaterial.color = hoverColor;
    }
}