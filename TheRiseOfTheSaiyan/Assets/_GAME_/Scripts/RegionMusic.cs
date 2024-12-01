using UnityEngine;

public class RegionMusic : MonoBehaviour
{
    [Header("Music for this region")]
    [SerializeField] private AudioClip regionMusic;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assuming Goku has the tag "Player"
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                audioManager.PlayMusic(regionMusic);
            }
        }
    }
}
