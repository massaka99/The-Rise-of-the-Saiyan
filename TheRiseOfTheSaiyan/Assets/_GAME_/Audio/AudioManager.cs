using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip background; 
    public AudioClip collision;
    public AudioClip walk;
    public AudioClip Teleport;

    private void Start()
    {
        PlayMusic(background); 
    }

    public void PlayMusic(AudioClip newMusic)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop(); 
        }
        if (musicSource.clip != newMusic) 
        {
            musicSource.clip = newMusic; 
            musicSource.Play(); 
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
