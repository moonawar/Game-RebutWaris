using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuManager
{
    [SerializeField] private GameObject SettingsScreen;

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
        SettingsScreen.SetActive(true);
    }

    public void Restart()
    {
        GameplayManager.Instance.UnpauseGame();
        FlowManager.Instance.RestartGame();
    }
}
