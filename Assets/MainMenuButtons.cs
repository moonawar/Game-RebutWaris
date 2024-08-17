using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class Screen
{
    public string ScreenName;
    public GameObject ScreenObject;
}
public class MenuManager : MonoBehaviour
{
    public void OnButtonPress()
    {
        AudioManager.Instance.PlaySFX("ButtonPress");
    }

    public void OnButtonHover()
    {
        AudioManager.Instance.PlaySFX("ButtonHover");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
public class MainMenuButtons : MenuManager
{

    [SerializeField] private Button PlayButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private GameObject MenuScreen;
    [SerializeField] private GameObject TutorialScreen;
    [SerializeField] private GameObject SettingsScreen;
    [SerializeField] private Button TutorialResumeButton;
    [SerializeField] private Button SettingsResumeButton;
    private MainMenuTransition MainMenuTransition;
    private bool isButtonActive = false;

    private void Awake()
    {
        TutorialScreen.SetActive(false);
        //SettingsScreen.SetActive(true);
        SettingsScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }

        if (!isButtonActive && MainMenuTransition.finished && (Input.anyKeyDown || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            isButtonActive = true;
            EventSystem.current.SetSelectedGameObject(PlayButton.gameObject, new BaseEventData(EventSystem.current));
        } 
        else if(isButtonActive && EventSystem.current.currentSelectedGameObject == null && (Input.anyKeyDown || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            isButtonActive = false;
        } 
    }

    private void OnEnable()
    {
        MainMenuTransition = gameObject.GetComponent<MainMenuTransition>();
    }

    private void Start()
    {
        MainMenuTransition = gameObject.GetComponent<MainMenuTransition>();
    }

    public void Play()
    {
        AudioManager.Instance.ChangeVolume("Static", 1);
        AudioManager.Instance.StopSFX("Static");
        AudioManager.Instance.StopBGM();
        FlowManager.Instance.LoadGameScene();
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
        if (TutorialScreen.activeInHierarchy)
        {
            DeactivateTutorial();
        }
        else
        {
            ActivateTutorial();
        }
        SettingsScreen.SetActive(false);
    }

    public void Settings()
    {
        DeactivateTutorial();
        SettingsScreen.SetActive(true);
        MenuScreen.SetActive(false);
        TutorialScreen.SetActive(false);
    }

    public void BackToMenu()
    {
        DeactivateTutorial();
        SettingsScreen.SetActive(false);
        MenuScreen.SetActive(true);
        TutorialScreen.SetActive(false);
    }
}
