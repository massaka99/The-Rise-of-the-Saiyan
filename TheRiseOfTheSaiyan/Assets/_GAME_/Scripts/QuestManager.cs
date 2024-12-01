using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    
    // Chichi's Quest
    public int saibamenKilled { get; private set; }
    public bool isQuestActive { get; private set; }
    public bool isQuestCompleted { get; private set; }
    private const int REQUIRED_KILLS = 10;

    // Gohan's Quest
    public bool isVegetaQuestActive { get; private set; }
    public bool isVegetaQuestCompleted { get; private set; }
    public bool hasSpokenToGohanBeforeChichiQuest { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Chichi's Quest Methods
    public void StartSaibamenQuest()
    {
        isQuestActive = true;
        saibamenKilled = 0;
        isQuestCompleted = false;
        Debug.Log("Quest Started: Kill 10 Saibamen");
    }

    public void IncrementSaibamenKilled()
    {
        if (isQuestActive && !isQuestCompleted)
        {
            saibamenKilled++;
            Debug.Log($"Saibamen killed: {saibamenKilled}/{REQUIRED_KILLS}");

            if (saibamenKilled >= REQUIRED_KILLS)
            {
                isQuestCompleted = true;
                Debug.Log("Quest Completed!");
            }
        }
    }

    // Gohan's Quest Methods
    public void StartVegetaQuest()
    {
        if (isQuestCompleted) // Only if Chichi's quest is completed
        {
            isVegetaQuestActive = true;
            isVegetaQuestCompleted = false;
            Debug.Log("Vegeta Quest Started");
        }
    }

    public void CompleteVegetaQuest()
    {
        if (isVegetaQuestActive && !isVegetaQuestCompleted)
        {
            isVegetaQuestCompleted = true;
            Debug.Log("Vegeta Quest Completed!");
        }
    }

    public void SetGohanFirstInteraction()
    {
        hasSpokenToGohanBeforeChichiQuest = true;
    }

    public void ResetQuests()
    {
        // Reset Chichi's Quest
        isQuestActive = false;
        saibamenKilled = 0;
        isQuestCompleted = false;

        // Reset Gohan's Quest
        isVegetaQuestActive = false;
        isVegetaQuestCompleted = false;
        hasSpokenToGohanBeforeChichiQuest = false;
    }
} 