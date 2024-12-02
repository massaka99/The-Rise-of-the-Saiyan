using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControllerReturn : TeleportControllerBase
{
    [SerializeField] private Dialog cantTeleportDialog;
    [SerializeField] private Dialog mustDefeatVegetaDialog;
    private static bool isInBossArea = false;

    protected override void Awake()
    {
        base.Awake();
        if (cantTeleportDialog == null || mustDefeatVegetaDialog == null)
        {
            Debug.LogError("Please assign both Dialogs in the Unity Inspector for TeleportControllerReturn");
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

        // If we're in the boss area, check if Vegeta is defeated
        if (isInBossArea)
        {
            bool canLeave = QuestManager.Instance.isVegetaQuestCompleted;
            Debug.Log($"Trying to leave boss area. Vegeta defeated: {canLeave}");
            return canLeave;
        }
        // If we're in the normal area, check if Saibamen quest is completed
        else
        {
            return QuestManager.Instance.isQuestCompleted;
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
                Dialog dialogToShow = isInBossArea ? mustDefeatVegetaDialog : cantTeleportDialog;
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(dialogToShow));
                return;
            }
            base.OnTriggerEnter2D(other);
        }
    }
} 