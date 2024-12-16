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

    // Level 2 Quests
    public int level2SaibamenKilled { get; private set; }
    public bool isLevel2QuestActive { get; private set; }
    public bool isLevel2SaibamenQuestCompleted { get; private set; }
    private const int LEVEL2_REQUIRED_KILLS = 20;

    // Boss States
    public bool isFirstBossDefeated { get; private set; }
    public bool isSecondBossDefeated { get; private set; }
    public bool isThirdBossDefeated { get; private set; }
    public bool canFightBosses { get; private set; }

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

    public void StartLevel2Quest()
    {
        isLevel2QuestActive = true;
        level2SaibamenKilled = 0;
        isLevel2SaibamenQuestCompleted = false;
        canFightBosses = false;
        Debug.Log("Level 2 Quest Started: Kill 20 Saibamen");
    }

    public void IncrementLevel2SaibamenKilled()
    {
        if (isLevel2QuestActive && !isLevel2SaibamenQuestCompleted)
        {
            level2SaibamenKilled++;
            Debug.Log($"Level 2 Saibamen killed: {level2SaibamenKilled}/{LEVEL2_REQUIRED_KILLS}");

            if (level2SaibamenKilled >= LEVEL2_REQUIRED_KILLS)
            {
                isLevel2SaibamenQuestCompleted = true;
                Debug.Log("Level 2 Saibamen Quest Completed!");
            }
        }
    }

    public void EnableBossFights()
    {
        canFightBosses = true;
    }

    public void SetBossDefeated(int bossNumber)
    {
        switch (bossNumber)
        {
            case 1:
                isFirstBossDefeated = true;
                break;
            case 2:
                isSecondBossDefeated = true;
                break;
            case 3:
                isThirdBossDefeated = true;
                break;
        }
    }

    public bool CanFightNextBoss(int bossNumber)
    {
        if (!canFightBosses || !isLevel2SaibamenQuestCompleted) 
            return false;

        switch (bossNumber)
        {
            case 1: // Frieza
                return true;
            case 2: // Cell
                return isFirstBossDefeated;
            case 3: // Buu
                return isSecondBossDefeated;
            default:
                return false;
        }
    }
} 