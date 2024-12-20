using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    
    public int saibamenKilled { get; private set; }
    public bool isQuestActive { get; private set; }
    public bool isQuestCompleted { get; private set; }
    private const int REQUIRED_KILLS = 10;

    public bool isVegetaQuestActive { get; private set; }
    public bool isVegetaQuestCompleted { get; private set; }
    public bool hasSpokenToGohanBeforeChichiQuest { get; private set; }

    public int level2SaibamenKilled { get; private set; }
    public bool isLevel2QuestActive { get; private set; }
    public bool isLevel2SaibamenQuestCompleted { get; private set; }
    private const int LEVEL2_REQUIRED_KILLS = 20;

    public bool isFirstBossDefeated { get; private set; }
    public bool isSecondBossDefeated { get; private set; }
    public bool isThirdBossDefeated { get; private set; }
    public bool canFightBosses { get; private set; }

    private bool isFriezaQuestActive;
    private bool isCellQuestActive;
    private bool isBuuQuestActive;

    public bool IsFriezaQuestActive => isFriezaQuestActive;
    public bool IsCellQuestActive => isCellQuestActive;
    public bool IsBuuQuestActive => isBuuQuestActive;

    public bool isBeerusQuestActive { get; private set; }
    public bool isBeerusDefeated { get; private set; }

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
                Debug.Log("Chichi's Quest Completed!");
            }
        }
    }

    public void StartVegetaQuest()
    {
        if (isQuestCompleted) 
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
        isQuestActive = false;
        saibamenKilled = 0;
        isQuestCompleted = false;

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
        isFriezaQuestActive = true;
        isCellQuestActive = false;
        isBuuQuestActive = false;
    }

    public void SetBossDefeated(int bossNumber)
    {
        switch (bossNumber)
        {
            case 1:
                isFirstBossDefeated = true;
                isFriezaQuestActive = false;
                break;
            case 2:
                isSecondBossDefeated = true;
                isCellQuestActive = false;
                break;
            case 3:
                isThirdBossDefeated = true;
                isBuuQuestActive = false;
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
                return IsFriezaQuestActive;
            case 2: // Cell
                return IsCellQuestActive && isFirstBossDefeated;
            case 3: // Buu
                return IsBuuQuestActive && isSecondBossDefeated;
            default:
                return false;
        }
    }

    public bool IsBossQuestActive(int bossNumber)
    {
        switch (bossNumber)
        {
            case 1:
                return isFriezaQuestActive;
            case 2:
                return isCellQuestActive;
            case 3:
                return isBuuQuestActive;
            default:
                return false;
        }
    }

    public void StartCellQuest()
    {
        isCellQuestActive = true;
    }

    public void StartBuuQuest()
    {
        isBuuQuestActive = true;
    }

    public void StartBeerusQuest()
    {
        isBeerusQuestActive = true;
        isBeerusDefeated = false;
        Debug.Log("Beerus quest has begun!");
    }

    public void CompleteBeerusQuest()
    {
        if (isBeerusQuestActive && !isBeerusDefeated)
        {
            isBeerusDefeated = true;
            Debug.Log("Beerus has been defeated! Game Complete!");
            SceneManager.LoadSceneAsync(5);
        }
    }
} 