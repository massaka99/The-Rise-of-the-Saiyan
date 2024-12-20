using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportControllerToLevel3 : MonoBehaviour
{
    public int index;  
    [SerializeField] private Dialog cantProgressDialog;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (cantProgressDialog == null)
        {
            Debug.LogError("Please assign the Dialog in the Unity Inspector for TeleportControllerToLevel3");
        }
    }

    private bool CanProgress()
    {
        if (QuestManager.Instance == null) return false;
        
        return QuestManager.Instance.isThirdBossDefeated;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CanProgress())
            {
                if (QuestManager.Instance != null)
                {
                    QuestManager.Instance.StartBeerusQuest();
                    Debug.Log("Beerus quest initialized from Level 2 transition");
                }

                if (audioSource != null)
                {
                    audioSource.Play();
                }
                
                SceneManager.LoadScene(index);
            }
            else if (Dialog_Manager.Instance != null && cantProgressDialog != null)
            {
                StartCoroutine(Dialog_Manager.Instance.ShowDialog(cantProgressDialog));
            }
        }
    }
} 