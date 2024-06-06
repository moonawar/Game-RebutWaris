using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private bool isTimerActive = false;
    private double GameTimer;
    [SerializeField] private float GameTime;
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;
    [Header("End of Game")]
    [SerializeField] private GameplayUIRefs player1UiRefs;
    [SerializeField] private GameplayUIRefs player2UiRefs;
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

    public void Update()
    {
        if (isTimerActive)
        {
            GameTimer -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(GameTimer);
            string displayTime = time.ToString(@"mm\:ss");
            TimerText.text = displayTime;
        }
    }

    public bool GameEnded { get; private set; } = false;

    [HideInInspector] public List<GameObject> Players;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }

        GameEnded = false;
        // endOfGameScreen.SetActive(false);
    }

    public void PauseGame()
    {
        AudioManager.Instance.StopAllSFX();
        AudioManager.Instance.PlaySFX("Static");
        AudioManager.Instance.PauseBGM();
        Paused = true;
        pauseScreen.SetActive(true);
    }

    public void UnpauseGame()
    {
        AudioManager.Instance.StopSFX("Static");
        AudioManager.Instance.UnpauseBGM();
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
        GameEnded = true;
        AudioManager.Instance.StopAllSFX();
        AudioManager.Instance.PlayBGMCrossfade("EndScreen");
        EndGameScreenManager endGameScreenManager = endOfGameScreen.GetComponent<EndGameScreenManager>();
        endGameScreenManager.ShowTheWinner(winnerIdx);
    }
    
    public void StartGameTimer()
    {
        isTimerActive = true;
        GameTimer = GameTime;
        StartCoroutine(GameTimerCoroutine());
    }

    public IEnumerator GameTimerCoroutine()
    {
        yield return new WaitForSeconds(GameTime);
        if (Players[0].GetComponent<PlayerMash>().GetLoveValue() >= Players[0].GetComponent<PlayerMash>().GetLoveValue())
        {
            EndTheGame(0);
        }
        else
        {
            EndTheGame(1);
        }
        isTimerActive= false;
        
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