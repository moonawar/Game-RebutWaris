using System.Collections;
using UnityEngine;

public class LeafblowerEvent : GameEvent {
    [SerializeField] private float strength = 1f;
    private int direction;

    public override string GetName()
    {
        return "Leafblower Event!";
    }

    public override void OnEnter(GameEventsManager mgr)
    {
        direction = RandomDirection();
        mgr.Mika.StartLeafblowerEvent(-direction, onLeafblowerOn, onLeafblowerOff);
    }

    private void onLeafblowerOn() {
        GameplayManager.Instance.Players.ForEach(player => {
            player.GetComponent<PlayerMovement>().transformers.Add(LeafblowerTransformer);
        });
    }

    private void onLeafblowerOff() {
        GameplayManager.Instance.Players.ForEach(player => {
            player.GetComponent<PlayerMovement>().transformers.Remove(LeafblowerTransformer);
        });
    }

    private int RandomDirection()
    {
        return Random.Range(0, 2) == 0 ? PlayerMovement.LEFT : PlayerMovement.RIGHT;
    }

    private Vector3 LeafblowerTransformer(Vector3 move)
    {
        return move + new Vector3(direction * strength, 0, 0);
    }

    // Should be called by animation event
    public void OnMikaTurnOnLeafblower() {
        // Later
    }
}