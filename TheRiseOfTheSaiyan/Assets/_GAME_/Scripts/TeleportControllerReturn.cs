using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportControllerReturn : TeleportControllerBase
{
    [SerializeField] private Dialog cantTeleportDialog;
    [SerializeField] private Dialog mustDefeatVegetaDialog;
    [SerializeField] private Dialog mustDefeatBossDialog;
    private static bool isInBossArea = false;
    [SerializeField] private int currentBossNumber;

    protected override void Awake()
    {
        base.Awake();
        if (cantTeleportDialog == null || mustDefeatVegetaDialog == null || mustDefeatBossDialog == null)
        {
            Debug.LogError("Please assign all Dialogs in the Unity Inspector for TeleportControllerReturn");
        }
        isInBossArea = false;
    }

    public static void SetInBossArea(bool value)
    {
        isInBossArea = value;
        Debug.Log($"Boss area state changed: {isInBossArea}");
    }

    protected override bool CanTeleport()
    {
        if (QuestManager.Instance == null) return false;

        // Level 2 Logic
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (isInBossArea)
            {
                // Check if current boss is defeated
                switch (currentBossNumber)
                {
                    case 1:
                        bool canLeave = QuestManager.Instance.isFirstBossDefeated;
                        Debug.Log($"Checking if can leave Frieza's area: {canLeave}");
                        return canLeave;
                    case 2:
                        canLeave = QuestManager.Instance.isSecondBossDefeated;
                        Debug.Log($"Checking if can leave Cell's area: {canLeave}");
                        return canLeave;
                    case 3:
                        canLeave = QuestManager.Instance.isThirdBossDefeated;
                        Debug.Log($"Checking if can leave Buu's area: {canLeave}");
                        return canLeave;
                    default:
                        return false;
                }
            }
            return QuestManager.Instance.isLevel2SaibamenQuestCompleted;
        }
        
        // Level 1 Logic
        if (isInBossArea)
        {
            return QuestManager.Instance.isVegetaQuestCompleted;
        }
        return QuestManager.Instance.isQuestCompleted;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool canTeleport = CanTeleport();
            Debug.Log($"Return Teleporter - Can teleport: {canTeleport}, Is in boss area: {isInBossArea}");

            if (!canTeleport && Dialog_Manager.Instance != null)
            {
                Dialog dialogToShow;
                if (SceneManager.GetActiveScene().buildIndex == 3 && isInBossArea)
                {
                    dialogToShow = mustDefeatBossDialog;
                }
                else if (isInBossArea)
                {
                    dialogToShow = mustDefeatVegetaDialog;
                }
                else
                {
                    dialogToShow = cantTeleportDialog;
                }
                
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(dialogToShow));
                return;
            }
            base.OnTriggerEnter2D(other);
        }
    }
} 