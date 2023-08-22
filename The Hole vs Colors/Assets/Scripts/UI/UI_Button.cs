using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Button : MonoBehaviour
{
    public int sceneIndex;

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }


    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
