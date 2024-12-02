using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Level1Introduction : MonoBehaviour
{
    private readonly string welcomeMessage = "Welcome to Dragon Bounce X";
    private readonly string explorationMessage = "Are you ready to explore what the universe has to offer?";
    private readonly string controlsMessage = "Controls:\n\nMovement: W A S D\nSprint: SHIFT\nPunch: SPACE\nInteract: E";
    
    private bool isFirstLoad = true;

    private void Start()
    {
        // Only show introduction on first load
        if (isFirstLoad)
        {
            StartCoroutine(ShowIntroductionSequence());
            isFirstLoad = false;
        }
    }

    private IEnumerator ShowIntroductionSequence()
    {
        yield return new WaitForSeconds(1f);

        UIManager.Instance.ShowPopup(welcomeMessage, 3f, false);
        yield return new WaitForSeconds(3.5f);

        UIManager.Instance.ShowPopup(explorationMessage, 3f, false);
        yield return new WaitForSeconds(3.5f);

        UIManager.Instance.ShowPopup(controlsMessage, 5f, true);
    }
} 