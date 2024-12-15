using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC_Controller : MonoBehaviour, Interactable
{
    [SerializeField] private Dialog questStartDialog;
    [SerializeField] private Dialog questInProgressDialog;
    [SerializeField] private Dialog questCompletedDialog;

    [Header("Level 2")]
    [SerializeField] private Dialog level2CompletedDialog;

    private bool isInteracting = false;

    public void Interact()
    {
        if (isInteracting) return;
        isInteracting = true;

        if (QuestManager.Instance != null)
        {
            if (SceneManager.GetActiveScene().buildIndex == 3) // Level 2 is now index 3
            {
                if (QuestManager.Instance.isThirdBossDefeated)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(level2CompletedDialog));
                    StartCoroutine(LoadNextLevel());
                }
            }
            else
            {
                // Existing Level 1 logic
                if (!QuestManager.Instance.isQuestActive)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(questStartDialog));
                    QuestManager.Instance.StartSaibamenQuest();
                }
                else if (QuestManager.Instance.isQuestCompleted)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(questCompletedDialog));
                }
                else
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(questInProgressDialog));
                }
            }
        }

        StartCoroutine(ResetInteraction());
    }

    private System.Collections.IEnumerator ResetInteraction()
    {
        yield return new WaitForSeconds(1);
        isInteracting = false;
    }

    private System.Collections.IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
