using System.Collections;
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
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                if (QuestManager.Instance.isThirdBossDefeated)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(level2CompletedDialog));
                }
            }
            else
            {
                if (!QuestManager.Instance.isQuestActive && !QuestManager.Instance.isQuestCompleted)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(questStartDialog));
                    QuestManager.Instance.StartSaibamenQuest();
                    Debug.Log("Chichi's Quest Started: Kill 10 Saibamen");
                }
                else if (QuestManager.Instance.isQuestActive && !QuestManager.Instance.isQuestCompleted)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(questInProgressDialog));
                    Debug.Log($"Current Saibamen killed: {QuestManager.Instance.saibamenKilled}/10");
                }
                else if (QuestManager.Instance.isQuestCompleted)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(questCompletedDialog));
                    Debug.Log("Chichi's Quest is completed!");
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
}
