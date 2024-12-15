using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportControllerMonster : TeleportControllerBase
{
    [SerializeField] private Dialog cantTeleportDialog;

    protected override void Awake()
    {
        base.Awake();
        if (cantTeleportDialog == null)
        {
            Debug.LogError("Please assign a Dialog in the Unity Inspector for TeleportControllerMonster");
        }
    }

    protected override bool CanTeleport()
    {
        if (QuestManager.Instance == null) return false;

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            return QuestManager.Instance.isLevel2QuestActive;
        }
        else
        {
            return QuestManager.Instance.isQuestActive;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!CanTeleport() && Dialog_Manager.Instance != null)
            {
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(cantTeleportDialog));
                return;
            }
            base.OnTriggerEnter2D(other);
        }
    }
}