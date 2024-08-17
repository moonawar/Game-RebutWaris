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
    private bool isButtonActive = false;

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
        isButtonActive = false;
    }

    private void OnDisable() {
        EventSystem.current.SetSelectedGameObject(SettingButton.gameObject, new BaseEventData(EventSystem.current));
    }

    private void Update()
    {
        if (!isButtonActive && (Input.anyKeyDown || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            isButtonActive = true;
            EventSystem.current.SetSelectedGameObject(ResumeButton.gameObject, new BaseEventData(EventSystem.current));
        }
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
