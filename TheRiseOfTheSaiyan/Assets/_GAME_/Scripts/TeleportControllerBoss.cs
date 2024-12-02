using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControllerBoss : TeleportControllerBase
{
    [SerializeField] private Dialog questNotCompletedDialog;
    [SerializeField] private Dialog gohanNotSpokenDialog;
    private bool hasEnteredBossArea = false;

    protected override void Awake()
    {
        base.Awake();
        ValidateDialogs();
    }

    private void ValidateDialogs()
    {
        if (questNotCompletedDialog == null || questNotCompletedDialog.Lines.Count == 0)
        {
            Debug.LogError("questNotCompletedDialog is not set up properly in TeleportControllerBoss");
        }
        if (gohanNotSpokenDialog == null || gohanNotSpokenDialog.Lines.Count == 0)
        {
            Debug.LogError("gohanNotSpokenDialog is not set up properly in TeleportControllerBoss");
        }
    }

    protected override bool CanTeleport()
    {
        if (QuestManager.Instance == null) 
        {
            Debug.LogError("QuestManager.Instance is null!");
            return false;
        }

        // For entering boss area
        if (!hasEnteredBossArea)
        {
            bool canEnter = QuestManager.Instance.isQuestCompleted && 
                           QuestManager.Instance.isVegetaQuestActive;
            Debug.Log($"Boss Teleporter - Quest Completed: {QuestManager.Instance.isQuestCompleted}, Vegeta Quest Active: {QuestManager.Instance.isVegetaQuestActive}, Can Enter: {canEnter}");
            return canEnter;
        }
        // For leaving boss area
        else
        {
            return QuestManager.Instance.isVegetaQuestCompleted;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool canTeleport = CanTeleport();
            Debug.Log($"Can teleport: {canTeleport}");

            if (canTeleport)
            {
                if (audioSource != null)
                {
                    audioSource.Play();
                }
                hasEnteredBossArea = !hasEnteredBossArea;
                TeleportControllerReturn.SetInBossArea(hasEnteredBossArea);
                other.transform.position = teleportDestination.position;
            }
            else if (Dialog_Manager.Instance != null)
            {
                Dialog dialogToShow = !QuestManager.Instance.isQuestCompleted ? 
                    questNotCompletedDialog : gohanNotSpokenDialog;

                if (dialogToShow != null && dialogToShow.Lines.Count > 0)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(dialogToShow));
                }
            }
        }
    }
}