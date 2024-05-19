using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{

    [SerializeField] private Button PlayButton;
    [SerializeField] private GameObject MenuScreen;
    [SerializeField] private GameObject TutorialScreen;
    [SerializeField] private GameObject SettingsScreen;
    [SerializeField] private Button TutorialResumeButton;
    [SerializeField] private Button SettingsResumeButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    private void OnEnable()
    {
        PlayButton.Select();

    }

    private void Start()
    {
        PlayButton.Select();
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Tutorial()
    {
        SettingsScreen.SetActive(false);
        MenuScreen.SetActive(false);
        TutorialScreen.SetActive(true);
    }

    public void Settings()
    {
        SettingsScreen.SetActive(true);
        MenuScreen.SetActive(false);
        TutorialScreen.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        SettingsScreen.SetActive(false);
        MenuScreen.SetActive(true);
        TutorialScreen.SetActive(false);
    }
}
