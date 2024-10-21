using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControllerBoss : MonoBehaviour
{
    public Transform teleportDestination;
    private AudioSource audioSource; 

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }

            other.transform.position = teleportDestination.position;
        }
    }
}
