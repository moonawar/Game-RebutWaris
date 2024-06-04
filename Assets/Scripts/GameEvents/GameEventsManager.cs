using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    [SerializeField] private GameEvent breadcrumbEvent;

    [Header("Event Trigger")]
    [SerializeField] private Range timeBetweenEvent;

    private float timer;
    private int currentGamePhase;

    [SerializeField] private MikaEvent mika;
    public MikaEvent Mika => mika;

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

        if (GameplayManager.Instance.GameEnded) {
            enabled = false;
        }
    }

    public void TriggerEvent() {
        if (_eventsOnPhases[currentGamePhase].gameEvents.Count > 0) {
            int randomEventIndex = Random.Range(0, _eventsOnPhases[currentGamePhase].gameEvents.Count);
            GameEvent eventToTrigger = _eventsOnPhases[currentGamePhase].gameEvents[randomEventIndex];
            eventToTrigger.OnEnter(this);
        }
    }

    public void OnGameChangePhase()
    {
        currentGamePhase = PhaseManager.Instance.CurrentPhase;
    }

    public void LeafblowerEvent()
    {
        leafblowerEvent.OnEnter(this);
    }

    public void BreadcrumbEvent()
    {
        breadcrumbEvent.OnEnter(this);
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

        if (GUILayout.Button("Trigger Random Event"))
        {
            gameEventsManager.TriggerEvent();
        }

        if (GUILayout.Button("Trigger Leafblower Event"))
        {
            gameEventsManager.LeafblowerEvent();
        }

        if (GUILayout.Button("Trigger Breadcrumb Event"))
        {
            gameEventsManager.BreadcrumbEvent();
        }
    }
}
#endif