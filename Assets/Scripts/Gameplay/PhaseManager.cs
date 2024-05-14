using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Phase {
    public float limit;
    public float increase;
}

public class PhaseManager : MonoBehaviour {
    public static PhaseManager Instance;

    [Header("Detection")]
    public float DetectRadius = 4f;
    public float DetectOffsetY = 0.0f;
    public float DetectOffsetX = 0.0f;

    [Header("Phase Properties")]
    public Phase[] GamePhases = new Phase[3];
    public float DecreaseRate = 0.5f;
    public float TimeToStartDecreasing = 3f;
    public int CurrentPhase {get; private set;}

    [Header("Events")]
    [SerializeField] private UnityEvent onPhaseChange;

    private Dictionary<int, int> _lovePhaseOfPlayer = new();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    /// <param name="playerIdx">player index</param>
    /// <param name="phaseLevel">the phase index of player after advancing to next phase</param>
    public void OnPlayerAdvancePhase(int playerIdx, int phaseLevel) {
        Debug.Log($"Player {playerIdx} advances to phase {phaseLevel}");
        if (CurrentPhase < phaseLevel) {
            CurrentPhase = phaseLevel;
            onPhaseChange?.Invoke();
        }

        if (_lovePhaseOfPlayer.ContainsKey(playerIdx)) {
            _lovePhaseOfPlayer[playerIdx] = phaseLevel;
        } else {
            _lovePhaseOfPlayer.Add(playerIdx, phaseLevel);
        }

        if (phaseLevel >= 3) {
            GameplayManager.Instance.EndTheGame(playerIdx);
            return;
        }
    }
}