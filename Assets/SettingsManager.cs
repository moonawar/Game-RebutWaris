using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MenuManager
{
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;


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
