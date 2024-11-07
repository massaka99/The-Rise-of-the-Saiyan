using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControllerBase : MonoBehaviour
{
    public Transform teleportDestination;
    private AudioSource audioSource;
    private Animator playerAnimator;
    private Player_Controller playerController;

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
            playerController = player.GetComponent<Player_Controller>();
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

            if (playerAnimator != null && playerController != null)
            {
                playerController.StartTeleport(); 
                StartCoroutine(TeleportAfterAnimation(other));
            }
        }
    }

    private IEnumerator TeleportAfterAnimation(Collider2D player)
    {
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);

        player.transform.position = teleportDestination.position;

        playerController.EndTeleport();
    }
}