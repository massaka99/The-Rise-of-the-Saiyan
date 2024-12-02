using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI popupText;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (popupPanel != null)
            {
                popupPanel.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPopup(string message, float duration, bool autoHide = true)
    {
        if (popupPanel == null || popupText == null) return;

        popupPanel.SetActive(true);
        popupText.text = message;

        if (autoHide)
        {
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }
            hideCoroutine = StartCoroutine(HidePopupAfterDelay(duration));
        }
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HidePopup();
    }

    public void HidePopup()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
} 