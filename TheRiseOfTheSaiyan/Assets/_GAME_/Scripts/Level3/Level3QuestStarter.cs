using UnityEngine;

public class Level3QuestStarter : MonoBehaviour
{
    private void Start()
    {
        if (QuestManager.Instance != null && !QuestManager.Instance.isBeerusQuestActive)
        {
            QuestManager.Instance.StartBeerusQuest();
            Debug.Log("Level 3 started - Beerus quest initialized from direct scene load");
        }
    }
}