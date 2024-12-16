using UnityEngine;

public class Level3QuestStarter : MonoBehaviour
{
    private void Start()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.StartBeerusQuest();
            Debug.Log("Level 3 started - Beerus quest initialized");
        }
        else
        {
            Debug.LogError("QuestManager instance not found!");
        }
    }
}