using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    public void LoadManeMenuScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void LoadDialogScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DialogScene");
    }

    public void LoadGameScene(string levelName)
    {
        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
}
