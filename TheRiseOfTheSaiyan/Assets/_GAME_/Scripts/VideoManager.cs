using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private int currentSceneIndex;
    private int targetSceneIndex;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoFinished;
        
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        targetSceneIndex = (currentSceneIndex == 1) ? 2 : 4;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadSceneAsync(targetSceneIndex);    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync(targetSceneIndex);
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}