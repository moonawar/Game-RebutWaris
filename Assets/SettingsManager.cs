using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    private void Awake()
    {
        foreach (Audio audio in AudioManager.Instance.Audios)
        {
            if (audio.Type == AudioType.BGM)
            {
                AudioManager.Instance.ChangeVolume(audio.Name, BGMSlider.value);
            }

            if (audio.Type == AudioType.SFX)
            {
                AudioManager.Instance.ChangeVolume(audio.Name, SFXSlider.value);
            }
        }
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
