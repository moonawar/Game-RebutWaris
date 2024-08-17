using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowManager : MonoBehaviour
{
    public static FlowManager Instance { get; private set; }
    private string currentScene;
    private Action onLoaderCallback;
    private bool _hasStarted;

    public bool hasStarted
    {
        get { return _hasStarted; }
        private set 
        { 
            _hasStarted = value; 
        }
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void LoadScene(string sceneName)
    {
        onLoaderCallback = () => {
            StartCoroutine(LoadSceneAsync(sceneName));
        };

        currentScene = "LoadingScreen";
        SceneManager.LoadScene("LoadingScreen");
        AudioManager.Instance.StopAllSFX();
        AudioManager.Instance.StopBGM();
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            yield return null;
        }
        currentScene = sceneName;
    }

    public void LoadGameScene()
    {
        LoadScene("Game");
        hasStarted = true;
    }

    public void LoadMainMenuScene()
    {
        LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoaderCallback()
    {
        if (Instance.onLoaderCallback != null)
        {
            Instance.onLoaderCallback();
            Instance.onLoaderCallback = null;
        }
    }
}