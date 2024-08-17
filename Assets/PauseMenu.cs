using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        FlowManager.Instance.LoadMainMenuScene();
    }

    public void Settings()
    {
        SettingsScreen.SetActive(true);
    }

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject, new BaseEventData(EventSystem.current));
        }
    }

    public void Restart()
    {
        GameplayManager.Instance.UnpauseGame();
        FlowManager.Instance.RestartGame();
    }
}
