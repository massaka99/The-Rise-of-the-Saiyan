using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Kill Counter")]
    public TextMeshProUGUI killCounterText;
    private int saibamenKills = 0;

    [Header("Quest Tracker")]
    public TextMeshProUGUI questTrackerText;
    public GameObject questPanel;
    
    // Singleton pattern
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        UpdateKillCounter();
        UpdateQuestTracker();
    }

    public void AddKill()
    {
        saibamenKills++;
        UpdateKillCounter();
    }

    private void UpdateKillCounter()
    {
        if (killCounterText != null)
        {
            killCounterText.text = $"Saibamen Defeated: {saibamenKills}";
        }
    }

    public void UpdateQuestTracker(string questDescription = "")
    {
        if (questTrackerText != null)
        {
            if (string.IsNullOrEmpty(questDescription))
            {
                questDescription = "No Active Quest";
                questPanel.SetActive(false);
            }
            else
            {
                questPanel.SetActive(true);
            }
            questTrackerText.text = questDescription;
        }
    }
} 