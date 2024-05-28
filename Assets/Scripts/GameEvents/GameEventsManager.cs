using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[System.Serializable]
public class GameEventsOnPhase {
    public List<GameEvent> gameEvents;
}

public class GameEventsManager : MonoBehaviour
{
    [Header("Events that Could Happen")]
    [SerializeField] private GameEventsOnPhase[] _eventsOnPhases;

    // For testing purposes
    [SerializeField] private GameEvent leafblowerEvent;

    [Header("Event Trigger")]
    [SerializeField] private Range timeBetweenEvent;

    private float timer;
    private int currentGamePhase;

    private void Awake() {
        currentGamePhase = 0;
        timer = timeBetweenEvent.RandomValue();
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            TriggerEvent();
            timer = timeBetweenEvent.RandomValue();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1)) {
            TriggerEvent();
        }
    }

    private void TriggerEvent() {
        if (_eventsOnPhases[currentGamePhase].gameEvents.Count > 0) {
            int randomEventIndex = Random.Range(0, _eventsOnPhases[currentGamePhase].gameEvents.Count);
            _eventsOnPhases[currentGamePhase].gameEvents[randomEventIndex].OnEnter(this);
            TextPopUpManager.Instance.SpawnEventText(_eventsOnPhases[currentGamePhase].ToString());
        }
    }

    public void OnGameChangePhase()
    {
        currentGamePhase = PhaseManager.Instance.CurrentPhase;
    }

    public void LeafblowerEvent()
    {
        leafblowerEvent.OnEnter(this);
        TextPopUpManager.Instance.SpawnEventText("Leafblower Event!");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameEventsManager))]
public class GameEventsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameEventsManager gameEventsManager = (GameEventsManager)target;

        if (GUILayout.Button("Trigger Leafblower Event"))
        {
            gameEventsManager.LeafblowerEvent();
        }
    }
}
#endif
