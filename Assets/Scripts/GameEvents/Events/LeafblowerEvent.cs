using UnityEngine;

public class LeafblowerEvent : GameEvent {
    [SerializeField] private float duration;
    [SerializeField] private float strength;
    private float timer;

    public override void Enter()
    {
        
    }

    // Should be called by animation event
    public void OnMikaTurnOnLeafblower() {
        
    }
}