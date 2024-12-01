using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip background; // Default background music
    public AudioClip collision;
    public AudioClip walk;
    public AudioClip Teleport;

    private void Start()
    {
        PlayMusic(background); // Start with default music
    }

    public void PlayMusic(AudioClip newMusic)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop(); // Stop current music
        }
        if (musicSource.clip != newMusic) // Check if the new clip is different
        {
            musicSource.clip = newMusic; // Assign new clip
            musicSource.Play(); // Play new music
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
