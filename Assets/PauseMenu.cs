using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        GameplayManager.Instance.UnpauseGame();
    }

    public void Home()
    {
        GameplayManager.Instance.UnpauseGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void Settings()
    {

    }

    public void Restart()
    {
        GameplayManager.Instance.UnpauseGame();
    }
}
