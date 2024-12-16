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
                        return QuestManager.Instance.isFirstBossDefeated;
                    case 2:
                        return QuestManager.Instance.isSecondBossDefeated;
                    case 3:
                        return QuestManager.Instance.isThirdBossDefeated;
                    default:
                        return false;
                }
            }
            return QuestManager.Instance.isLevel2SaibamenQuestCompleted;
        }
        
        // Level 1 Logic
        if (isInBossArea)
        {
            // For Vegeta's area
            return QuestManager.Instance.isVegetaQuestCompleted;
        }
        else
        {
            // For Saibamen area
            if (QuestManager.Instance.isQuestActive)
            {
                // Can return if quest is completed
                return QuestManager.Instance.isQuestCompleted;
            }
            // Can go to Saibamen area if quest is active
            return QuestManager.Instance.isQuestActive;
        }
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