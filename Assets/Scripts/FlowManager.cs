using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowManager : MonoBehaviour
{
    public static FlowManager Instance { get; private set; }
    private string currentScene;
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        currentScene = sceneName;
    }

    public void LoadGameScene()
    {
        LoadScene("Game");
    }

    public void LoadMainMenuScene()
    {
        LoadScene("MainMenu");
    }
}