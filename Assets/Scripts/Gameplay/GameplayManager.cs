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
    [SerializeField] private GameObject endOfGameScreen;

    public static GameplayManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }

        endOfGameScreen.SetActive(false);
    }

    public void RestartGame() {
        FlowManager.Instance.RestartGame();
    }

    public void EndTheGame(int winnerIdx) {
        endOfGameScreen.SetActive(true);
        if (winnerIdx == 0) {
            player1UiRefs.endGame.winningScreen.SetActive(true);
        } else {
            player2UiRefs.endGame.winningScreen.SetActive(true);
        }
    }
}
