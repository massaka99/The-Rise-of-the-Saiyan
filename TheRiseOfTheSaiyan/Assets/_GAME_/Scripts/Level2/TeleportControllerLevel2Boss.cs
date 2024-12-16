using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TeleportControllerLevel2Boss : TeleportControllerBase
{
    [SerializeField] private int bossNumber;  // Set this in inspector: 1, 2, or 3
    [SerializeField] private Dialog cantFightYetDialog;
    [SerializeField] private Dialog previousBossNotDefeatedDialog;
    [SerializeField] private Dialog saibamenQuestNotCompletedDialog;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!CanTeleport())
            {
                return;
            }
            
            if (audioSource != null)
            {
                audioSource.Play();
            }
            
            // Set boss area state before teleporting
            TeleportControllerReturn.SetInBossArea(true);
            other.transform.position = teleportDestination.position;
        }
    }

    protected override bool CanTeleport()
    {
        if (QuestManager.Instance == null) return false;

        // Use the QuestManager's CanFightNextBoss method
        if (!QuestManager.Instance.CanFightNextBoss(bossNumber))
        {
            // Show appropriate dialog based on the condition
            if (!QuestManager.Instance.isLevel2SaibamenQuestCompleted)
            {
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(saibamenQuestNotCompletedDialog));
            }
            else if (!QuestManager.Instance.canFightBosses)
            {
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(cantFightYetDialog));
            }
            else if (!QuestManager.Instance.IsBossQuestActive(bossNumber))
            {
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(cantFightYetDialog));
            }
            else
            {
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(previousBossNotDefeatedDialog));
            }
            return false;
        }

        return true;
    }
}