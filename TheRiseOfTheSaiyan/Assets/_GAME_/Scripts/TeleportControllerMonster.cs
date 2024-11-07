using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControllerMonster : MonoBehaviour
{
    public Transform teleportDestination;
    private AudioSource audioSource;
    private Animator playerAnimator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }

            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Teleport");
            }

            other.transform.position = teleportDestination.position;
        }
    }
}