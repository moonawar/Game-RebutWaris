using System.Collections;
using UnityEngine;

public class LeafblowerEvent : GameEvent {
    [SerializeField] private float duration = 5f;
    [SerializeField] private float strength = 1f;
    private int direction;

    public override string GetName()
    {
        return "Leafblower Event!";
    }

    public override void OnEnter(GameEventsManager mgr)
    {
        GameplayManager.Instance.Players.ForEach(player => {
            direction = RandomDirection();
            player.GetComponent<PlayerMovement>().transformers.Add(LeafblowerTransformer);
            mgr.StartCoroutine(UnregisterLeafblower(player.GetComponent<PlayerMovement>()));
        });
    }

    public IEnumerator UnregisterLeafblower(PlayerMovement player)
    {
        yield return new WaitForSeconds(duration);
        player.transformers.Remove(LeafblowerTransformer);
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