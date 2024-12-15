using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    public void Skip()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
