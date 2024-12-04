using UnityEngine;
using System.Collections;

public class Level1Introduction : MonoBehaviour
{
    private readonly string welcomeMessage = "Welcome to Dragon Bounce X";
    private readonly string explorationMessage = "Are you ready to explore what the universe has to offer?";
    private readonly string controlsMessage = "Controls:\n\nMovement: W A S D\nSprint: SHIFT\nPunch: SPACE\nInteract: E";
    
    private const string INTRO_SHOWN_KEY = "IntroShown";
    [SerializeField] private GameObject introCanvas;

    private void Start()
    {
        // Check if we've shown the intro before
        if (!PlayerPrefs.HasKey(INTRO_SHOWN_KEY))
        {
            introCanvas.SetActive(true);
            StartCoroutine(ShowIntroductionSequence());
            PlayerPrefs.SetInt(INTRO_SHOWN_KEY, 1);
            PlayerPrefs.Save();
        }
        else
        {
            // Find and destroy the Canvas in the hierarchy
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                Destroy(canvas);
            }

            // Also destroy the UIManager instance
            if (UIManager.Instance != null)
            {
                Destroy(UIManager.Instance.gameObject);
            }
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