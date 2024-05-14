using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameEventsOnPhase {
    public List<GameEvent> gameEvents;
}

public class GameEventsManager : MonoBehaviour
{
    [Header("Events that Could Happen")]
    [SerializeField] private GameEventsOnPhase _onPhase0;
    [SerializeField] private GameEventsOnPhase _onPhase1;
    [SerializeField] private GameEventsOnPhase _onPhase2;

    [Header("Event Trigger")]
    [SerializeField] private Range timeBetweenEvent;

    private int currentGamePhase;
    public void OnGameChangePhase()
    {
        currentGamePhase = PhaseManager.Instance.CurrentPhase;
    }
}
