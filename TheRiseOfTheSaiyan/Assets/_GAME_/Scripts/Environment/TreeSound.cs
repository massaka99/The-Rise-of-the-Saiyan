using UnityEngine;

public class TreeSound : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] leafRustlingSounds;
    [SerializeField] [Range(0f, 1f)] private float baseVolume = 0.2f;
    [SerializeField] [Range(0f, 20f)] private float maxDistance = 8f;
    [SerializeField] [Range(0f, 10f)] private float spatialBlend = 1f;
    
    [Header("Rustling Settings")]
    [SerializeField] [Range(0f, 10f)] private float minTimeBetweenSounds = 3f;
    [SerializeField] [Range(0f, 10f)] private float maxTimeBetweenSounds = 8f;
    [SerializeField] [Range(0f, 10f)] private float playerDetectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;

    private AudioSource audioSource;
    private float nextSoundTime;

    private void Awake()
    {
        // Create and configure AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = baseVolume;
        audioSource.spatialBlend = spatialBlend;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.playOnAwake = false;
        
        // Set initial next sound time
        nextSoundTime = Time.time + Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
    }

    private void Update()
    {
        if (Time.time >= nextSoundTime)
        {
            // Check if player is nearby
            bool playerNearby = Physics2D.OverlapCircle(transform.position, playerDetectionRadius, playerLayer);
            
            // Increase chance of sound if player is nearby
            if (playerNearby || Random.value < 0.3f)
            {
                PlayRandomTreeSound();
            }
            
            // Set next sound time
            nextSoundTime = Time.time + Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
        }
    }

    private void PlayRandomTreeSound()
    {
        if (leafRustlingSounds == null || leafRustlingSounds.Length == 0) return;

        // Pick random sound from array
        AudioClip randomClip = leafRustlingSounds[Random.Range(0, leafRustlingSounds.Length)];
        
        // Randomize volume slightly
        float randomVolume = baseVolume * Random.Range(0.8f, 1.2f);
        
        audioSource.clip = randomClip;
        audioSource.volume = randomVolume;
        audioSource.Play();
    }

    private void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.volume = baseVolume;
            audioSource.maxDistance = maxDistance;
            audioSource.spatialBlend = spatialBlend;
        }
    }

    // Optional: Visualize detection radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
} 