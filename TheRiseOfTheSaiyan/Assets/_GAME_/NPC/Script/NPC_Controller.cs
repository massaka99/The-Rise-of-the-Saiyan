using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour, Interactable
{
    [SerializeField] private Dialog questStartDialog;
    [SerializeField] private Dialog questInProgressDialog;
    [SerializeField] private Dialog questCompletedDialog;
    
    private bool isInteracting = false;

    public void Interact()
    {
        if (isInteracting) return;
        isInteracting = true;

        if (QuestManager.Instance != null)
        {
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

        StartCoroutine(ResetInteraction());
    }

    private System.Collections.IEnumerator ResetInteraction()
    {
        yield return new WaitForSeconds(1);
        isInteracting = false;
    }
}
