using UnityEngine;

public class WaterSound : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip waterAmbientSound;
    [SerializeField] [Range(0f, 1f)] private float volume = 0.3f;
    [SerializeField] [Range(0f, 50f)] private float maxDistance = 10f;
    [SerializeField] [Range(0f, 10f)] private float spatialBlend = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        // Create and configure AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = waterAmbientSound;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.spatialBlend = spatialBlend; // 1 = fully 3D sound
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f; // Disable doppler effect for ambient sound
        audioSource.playOnAwake = true;
        audioSource.Play();
    }

    private void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            audioSource.maxDistance = maxDistance;
            audioSource.spatialBlend = spatialBlend;
        }
    }
} 