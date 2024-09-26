using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControllerBoss : MonoBehaviour
{
    public Transform teleportDestination;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportDestination.position;
        }
    }
}
