using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gohan_Controller : MonoBehaviour, Interactable
{
    [Header("Level 1 Dialogs")]
    [SerializeField] private Dialog beforeChichiQuestDialog;
    [SerializeField] private Dialog remindChichiQuestDialog;
    [SerializeField] private Dialog questStartDialog;
    [SerializeField] private Dialog questInProgressDialog;
    [SerializeField] private Dialog questCompletedDialog;

    [Header("Level 2 Dialogs")]
    [SerializeField] private Dialog level2StartDialog;
    [SerializeField] private Dialog level2SaibamenQuestDialog;
    [SerializeField] private Dialog level2SaibamenCompletedDialog;
    [SerializeField] private Dialog boss1CompleteDialog;
    [SerializeField] private Dialog boss2CompleteDialog;
    [SerializeField] private Dialog boss3CompleteDialog;
    
    private bool isInteracting = false;

    public void Interact()
    {
        if (isInteracting) return;
        isInteracting = true;

        if (QuestManager.Instance != null)
        {
            // Level 2 Logic
            if (SceneManager.GetActiveScene().buildIndex == 3) // Level 2
            {
                HandleLevel2Interaction();
            }
            else // Level 1 Logic
            {
                HandleLevel1Interaction();
            }
        }

        StartCoroutine(ResetInteraction());
    }

    private void HandleLevel2Interaction()
    {
        if (!QuestManager.Instance.isLevel2QuestActive)
        {
            // Start Level 2 quest
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(level2StartDialog));
            QuestManager.Instance.StartLevel2Quest();
        }
        else if (!QuestManager.Instance.isLevel2SaibamenQuestCompleted)
        {
            // Remind about Saibamen quest
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(level2SaibamenQuestDialog));
        }
        else if (!QuestManager.Instance.canFightBosses)
        {
            // Enable boss fights after Saibamen quest completion
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(level2SaibamenCompletedDialog));
            QuestManager.Instance.EnableBossFights();
        }
        else if (QuestManager.Instance.isFirstBossDefeated && !QuestManager.Instance.isSecondBossDefeated)
        {
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(boss1CompleteDialog));
        }
        else if (QuestManager.Instance.isSecondBossDefeated && !QuestManager.Instance.isThirdBossDefeated)
        {
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(boss2CompleteDialog));
        }
        else if (QuestManager.Instance.isThirdBossDefeated)
        {
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(boss3CompleteDialog));
        }
    }

    private void HandleLevel1Interaction()
    {
        // Existing Level 1 logic
        if (!QuestManager.Instance.isQuestCompleted)
        {
            if (!QuestManager.Instance.hasSpokenToGohanBeforeChichiQuest && !QuestManager.Instance.isQuestActive)
            {
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(beforeChichiQuestDialog));
                QuestManager.Instance.SetGohanFirstInteraction();
            }
            else if (QuestManager.Instance.isQuestActive)
            {
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(remindChichiQuestDialog));
            }
        }
        else if (!QuestManager.Instance.isVegetaQuestActive)
        {
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(questStartDialog));
            QuestManager.Instance.StartVegetaQuest();
        }
        else if (QuestManager.Instance.isVegetaQuestCompleted)
        {
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(questCompletedDialog));
        }
        else
        {
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(questInProgressDialog));
        }
    }

    private System.Collections.IEnumerator ResetInteraction()
    {
        yield return new WaitForSeconds(1);
        isInteracting = false;
    }
} 