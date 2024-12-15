using System.Collections;
using UnityEngine;

public class TeleportControllerLevel2Boss : TeleportControllerBase
{
    [SerializeField] private int bossNumber;  // Set this in inspector: 1, 2, or 3
    [SerializeField] private Dialog cantFightYetDialog;
    [SerializeField] private Dialog previousBossNotDefeatedDialog;
    [SerializeField] private Dialog saibamenQuestNotCompletedDialog;

    protected override bool CanTeleport()
    {
        if (QuestManager.Instance == null) return false;

        // Check if player has completed Saibamen quest
        if (!QuestManager.Instance.isLevel2SaibamenQuestCompleted)
        {
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(saibamenQuestNotCompletedDialog));
            return false;
        }

        // Check if player can fight bosses (talked to Gohan)
        if (!QuestManager.Instance.canFightBosses)
        {
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(cantFightYetDialog));
            return false;
        }

        // Check boss sequence
        switch (bossNumber)
        {
            case 1:
                return true;
            case 2:
                if (!QuestManager.Instance.isFirstBossDefeated)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(previousBossNotDefeatedDialog));
                    return false;
                }
                return true;
            case 3:
                if (!QuestManager.Instance.isSecondBossDefeated)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(previousBossNotDefeatedDialog));
                    return false;
                }
                return true;
            default:
                return false;
        }
    }
} 