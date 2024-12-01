using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    
    public int saibamenKilled { get; private set; }
    public bool isQuestActive { get; private set; }
    public bool isQuestCompleted { get; private set; }
    private const int REQUIRED_KILLS = 10;

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
                Debug.Log("Quest Completed!");
            }
        }
    }

    public void ResetQuest()
    {
        isQuestActive = false;
        saibamenKilled = 0;
        isQuestCompleted = false;
    }
} 