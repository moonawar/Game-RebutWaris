using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsManager : MenuManager
{
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button SettingButton;
    private EventSystem _eventSystem;

    private void Awake()
    {
        foreach (Audio audio in AudioManager.Instance.Audios)
        {
            if (audio.Type == AudioType.BGM)
            {
                BGMSlider.value = audio.Volume;
            }

            if (audio.Type == AudioType.SFX)
            {
                SFXSlider.value = audio.Volume;
            }
        }
    }

    private void OnEnable() {
        _eventSystem = FindObjectOfType<EventSystem>();
        _eventSystem.SetSelectedGameObject(ResumeButton.gameObject);
    }

    private void OnDisable() {
        _eventSystem.SetSelectedGameObject(SettingButton.gameObject);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnBGMValueChange()
    {
        foreach(Audio audio in AudioManager.Instance.Audios)
        {
            if(audio.Type == AudioType.BGM)
            {
                AudioManager.Instance.ChangeVolume(audio.Name, BGMSlider.value);
            }
        }
    }

    public void OnSFXValueChange()
    {
        foreach (Audio audio in AudioManager.Instance.Audios)
        {
            if (audio.Type == AudioType.SFX)
            {
                AudioManager.Instance.ChangeVolume(audio.Name, SFXSlider.value);
            }
        }
    }
}
