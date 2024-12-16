using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public int index;
    [SerializeField] private Dialog cantProgressDialog;

    private void Awake()
    {
        if (cantProgressDialog == null)
        {
            Debug.LogError("Please assign the Dialog in the Unity Inspector for LevelController");
        }
    }

    private bool CanProgress()
    {
        if (QuestManager.Instance == null) return false;
        
        // For Level 1 to Level 2 transition
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            return QuestManager.Instance.isQuestCompleted && QuestManager.Instance.isVegetaQuestCompleted;
        }
        // For Level 2 to next level transition
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            return QuestManager.Instance.isThirdBossDefeated;
        }
        
        return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CanProgress())
            {
                SceneManager.LoadScene(index);
            }
            else if (Dialog_Manager.Instance != null && cantProgressDialog != null)
            {
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(cantProgressDialog));
            }
        }
    }
}
