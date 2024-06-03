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

    public void OnButtonPress()
    {
        AudioManager.Instance.PlaySFX("ButtonPress");
    }

    public void OnButtonHover()
    {
        AudioManager.Instance.PlaySFX("ButtonHover");
    }

    public void Play()
    {
        AudioManager.Instance.ChangeVolume("Static", 1);
        AudioManager.Instance.StopSFX("Static");
        SceneManager.LoadScene("Game");
    }

    private void ActivateTutorial()
    {
        gameObject.GetComponent<TutorialManager>().Activate();
        TutorialScreen.SetActive(true);
    }

    private void DeactivateTutorial()
    {
        GetComponent<TutorialManager>().Deactivate();
        TutorialScreen.SetActive(false);
    }

    public void Tutorial()
    {
        ActivateTutorial();
        SettingsScreen.SetActive(false);
        //MenuScreen.SetActive(false);
    }

    public void Settings()
    {
        DeactivateTutorial();
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
        DeactivateTutorial();
        SettingsScreen.SetActive(false);
        MenuScreen.SetActive(true);
        TutorialScreen.SetActive(false);
    }
}
