using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{

    [SerializeField] private Button PlayButton;
    [SerializeField] private Button TutorialButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button QuitButton;

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Tutorial()
    {

    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
