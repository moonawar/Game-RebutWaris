using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EndGameUIRefs {
    public GameObject winningScreen;
}

[System.Serializable]
public class GameplayUIRefs {
    public EndGameUIRefs endGame;
}

public class GameplayManager : MonoBehaviour
{
    [Header("End of Game")]
    [SerializeField] private GameplayUIRefs player1UiRefs;
    [SerializeField] private GameplayUIRefs player2UiRefs;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject endOfGameScreen;

    public static GameplayManager Instance { get; private set; }
    private bool _paused;
    public bool Paused {
        get { return _paused; }
        set { 
            _paused = value;
            Time.timeScale = value ? 0 : 1; 
        }
    }

    [HideInInspector] public List<GameObject> Players;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }

        // endOfGameScreen.SetActive(false);
    }

    public void PauseGame()
    {
        AudioManager.Instance.PlaySFX("Static");
        Paused = true;
        pauseScreen.SetActive(true);
    }

    public void UnpauseGame()
    {
        AudioManager.Instance.StopSFX("Static");
        Paused = false;
        pauseScreen.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsScreen.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsScreen.SetActive(false);
    }

    public void RestartGame() {
        FlowManager.Instance.RestartGame();
    }

    public void EndTheGame(int winnerIdx) {
        endOfGameScreen.SetActive(true);
        EndGameScreenManager endGameScreenManager = endOfGameScreen.GetComponent<EndGameScreenManager>();
        endGameScreenManager.ShowTheWinner(winnerIdx);
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(GameplayManager))]
public class GameplayManagerEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        GameplayManager gMan = (GameplayManager)target;

        string buttonText = gMan.Paused ? "Resume" : "Pause";
        if (GUILayout.Button(buttonText)) {
            gMan.Paused = !gMan.Paused;
        }
    }
}
#endif