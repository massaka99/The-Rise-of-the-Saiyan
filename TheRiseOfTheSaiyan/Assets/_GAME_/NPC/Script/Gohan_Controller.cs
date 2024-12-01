using System.Collections;
using UnityEngine;

public class Gohan_Controller : MonoBehaviour, Interactable
{
    [SerializeField] private Dialog beforeChichiQuestDialog;
    [SerializeField] private Dialog remindChichiQuestDialog;
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
            if (!QuestManager.Instance.isQuestCompleted)
            {
                // If Chichi's quest isn't completed
                if (!QuestManager.Instance.hasSpokenToGohanBeforeChichiQuest)
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(beforeChichiQuestDialog));
                    QuestManager.Instance.SetGohanFirstInteraction();
                }
                else
                {
                    StartCoroutine(Dialog_Manager.Instance.ShowDialog(remindChichiQuestDialog));
                }
            }
            else if (!QuestManager.Instance.isVegetaQuestActive)
            {
                // Start Vegeta quest after Chichi's quest is completed
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

        StartCoroutine(ResetInteraction());
    }

    private System.Collections.IEnumerator ResetInteraction()
    {
        yield return new WaitForSeconds(1);
        isInteracting = false;
    }
} 