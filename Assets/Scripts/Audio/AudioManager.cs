using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType { BGM, SFX }

[System.Serializable]
public class Audio {
    public string Name;
    public AudioClip Clip;
    public AudioType Type;
    [Range(0f, 1f)]
    public float Volume = 1f;
    [Range(0.1f, 3f)]
    public float Pitch = 1f;
    public bool Loop = false;
    [HideInInspector]
    public AudioSource Source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public List<Audio> Audios;
    private Audio currentBGM;

    // We need two sources to play the next BGM while the current one is fading out
    private AudioSource bgmSourceOne;
    private AudioSource bgmSourceTwo;
    private int activeBGMSource = 1;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        bgmSourceOne = gameObject.AddComponent<AudioSource>();
        bgmSourceTwo = gameObject.AddComponent<AudioSource>();

        foreach (Audio audio in Audios) {
            if (audio.Type == AudioType.SFX) {
                // SFX are okay to be overlapped, so each SFX can have its own source
                audio.Source = gameObject.AddComponent<AudioSource>();
                audio.Source.clip = audio.Clip;
                audio.Source.volume = audio.Volume;
                audio.Source.pitch = audio.Pitch;
                audio.Source.loop = audio.Loop;
            }

            // For the BGM we will use the two sources we have
        }
    }

    private Audio FindAudio(string name) {
        return Audios.Find(audio => audio.Name == name);
    }

    public void PlaySFX(string name) {
        Audio audio = FindAudio(name);
        if (audio == null) {
            Debug.LogWarning("AudioManager: SFX " + name + " not found!");
            return;
        }

        audio.Source.Play();
    }

    public void PlayBGMOverwrite(string name) {
        Audio audio = FindAudio(name);
        if (audio == null) {
            Debug.LogWarning("AudioManager: BGM " + name + " not found!");
            return;
        }

        if (audio.Type != AudioType.BGM) {
            Debug.LogWarning("AudioManager: " + name + " is not a BGM! Use PlaySFX instead.");
            return;
        }

        if (currentBGM != null) {
            AudioSource bgmSource = activeBGMSource == 1 ? bgmSourceOne : bgmSourceTwo;
            bgmSource.Stop();
        }

        currentBGM = audio;
        currentBGM.Source = activeBGMSource == 1 ? bgmSourceOne : bgmSourceTwo;
        activeBGMSource = activeBGMSource == 1 ? 2 : 1;

        currentBGM.Source.clip = currentBGM.Clip;
        currentBGM.Source.volume = currentBGM.Volume;
        currentBGM.Source.pitch = currentBGM.Pitch;
        currentBGM.Source.loop = currentBGM.Loop;
        currentBGM.Source.Play();
    }

    private IEnumerator FadeIn(Audio audio, int bgmSource, float duration) {
        AudioSource audioSource = bgmSource == 1 ? bgmSourceOne : bgmSourceTwo;
        audio.Source = audioSource;
        audio.Source.clip = audio.Clip;
        audio.Source.pitch = audio.Pitch;
        audio.Source.loop = audio.Loop;
        audio.Source.volume = 0f;
        audio.Source.Play();

        float timer = 0f;
        while (timer < duration) {
            audio.Source.volume = Mathf.Lerp(0f, audio.Volume, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        audio.Source.volume = audio.Volume;
    }

    private IEnumerator FadeOut(Audio audio, float duration) {
        float startVolume = audio.Source.volume;

        float timer = 0f;
        while (timer < duration) {
            audio.Source.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        audio.Source.volume = 0f;
        audio.Source.Stop();
    }

    /// <summary>
    /// crossfade between two BGMs
    /// </summary>
    /// <param name="name">name of the audio, must be defined via inspector</param>
    /// <param name="duration">duration of each fade, if put 1, then 1 second to fade out and 1 second to fade in</param>
    /// <param name="overlap">how many seconds the two BGMs will overlap</param>
    private IEnumerator PlayBGMCrossfadeCoroutine(string name, float duration, float overlap) {
        Audio audio = FindAudio(name);

        if (currentBGM == null) {
            StartCoroutine(FadeIn(audio, 1, duration));
        }

        float waitTime = duration - overlap;
        int nextBGMSource = activeBGMSource == 1 ? 2 : 1;

        StartCoroutine(FadeOut(currentBGM, duration));
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FadeIn(audio, nextBGMSource, duration));
    }

    public void PlayBGMCrossfade(string name, float duration = 1f, float overlap = 0f) {
        StartCoroutine(PlayBGMCrossfadeCoroutine(name, duration, overlap));
    }

    private IEnumerator PlayBGMAfterLoopEndCoroutine(string name) {
        Audio audio = FindAudio(name);

        if (currentBGM == null) {
            PlayBGMOverwrite(name);
        }

        yield return new WaitForSeconds(currentBGM.Clip.length - currentBGM.Source.time);
        PlayBGMOverwrite(name);
    }

    public void PlayBGMAfterLoopEnd(string name) {
        StartCoroutine(PlayBGMAfterLoopEndCoroutine(name));
    }

    public void StopBGM() {
        if (currentBGM != null) {
            currentBGM.Source.Stop();
            currentBGM = null;
        }
    }

    public void PlaySFX(string name, float volume) {
        Audio audio = FindAudio(name);
        if (audio == null) {
            Debug.LogWarning("AudioManager: SFX " + name + " not found!");
            return;
        }

        audio.Source.volume = volume;
        audio.Source.Play();
    }

    public void PlayRandomSFX(string[] names) {
        string name = names[Random.Range(0, names.Length)];
        PlaySFX(name);
    }
}
