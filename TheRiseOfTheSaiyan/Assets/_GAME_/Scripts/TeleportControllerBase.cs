using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControllerBase : MonoBehaviour
{
    public Transform teleportDestination;
    protected AudioSource audioSource;
    protected Animator playerAnimator;
    
    protected virtual bool CanTeleport()
    {
        return true; // Base implementation always allows teleport
    }

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CanTeleport())
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }
            other.transform.position = teleportDestination.position;
        }
    }
}