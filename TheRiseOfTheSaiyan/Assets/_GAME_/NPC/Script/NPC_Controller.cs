using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour, Interactable
{
    [SerializeField] private List<Dialog> dialogs;
    private int currentDialogIndex = 0;
    private bool isLocked = false;
    private bool isInteracting = false;

    public void Interact()
    {
        if (isInteracting) return;

        isInteracting = true;
        if (!isLocked)
        {
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(dialogs[currentDialogIndex]));

            if (currentDialogIndex < dialogs.Count - 1)
            {
                currentDialogIndex++;
            }
            else
            {
                isLocked = true;
            }
        }
        else
        {
            StartCoroutine(Dialog_Manager.Instance.ShowDialog(dialogs[currentDialogIndex]));
        }

        StartCoroutine(ResetInteraction());
    }

    private System.Collections.IEnumerator ResetInteraction()
    {
        yield return new WaitForSeconds(1); // Adjust time as needed
        isInteracting = false;
    }

    public void UnlockDialog()
    {
        isLocked = false;
        currentDialogIndex = 0;
    }
}
