using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beerus_Controller : MonoBehaviour, Interactable
{
    [SerializeField] private Dialog beerusDialog;
    private bool isInteracting = false;
    private bool isShowingDialog = false;

    public void Interact()
    {
        if (isInteracting || isShowingDialog) return;
        isInteracting = true;

        if (QuestManager.Instance != null)
        {
            HandleBeerusInteraction();
        }

        StartCoroutine(ResetInteraction());
    }

    private void HandleBeerusInteraction()
    {
        if (!isShowingDialog)
        {
            StartCoroutine(HandleDialog());
        }
    }

    private IEnumerator HandleDialog()
    {
        if (Dialog_Manager.Instance != null && beerusDialog != null)
        {
            isShowingDialog = true;
            
            Dialog_Manager.Instance.OnHideDialog += OnDialogComplete;
            yield return StartCoroutine(Dialog_Manager.Instance.ShowDialog(beerusDialog));
        }
    }

    private void OnDialogComplete()
    {
        Dialog_Manager.Instance.OnHideDialog -= OnDialogComplete;
        isShowingDialog = false;
        StartCoroutine(LoadOutroScene());
    }

    private IEnumerator LoadOutroScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadSceneAsync(6);
    }

    private IEnumerator ResetInteraction()
    {
        yield return new WaitForSeconds(1);
        isInteracting = false;
    }

    private void OnDestroy()
    {
        if (Dialog_Manager.Instance != null)
        {
            Dialog_Manager.Instance.OnHideDialog -= OnDialogComplete;
        }
    }
} 