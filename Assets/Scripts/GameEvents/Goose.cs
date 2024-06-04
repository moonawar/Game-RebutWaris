using UnityEngine;

public class Goose : MonoBehaviour
{
    private readonly float stunDuration = 2;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerMovement>().Stun(stunDuration);
            if (other.GetComponent<PlayerMash>().HaveClock)
            {
                other.GetComponent<PlayerMash>().HaveClock = false;
                ClockManager.Instance.SpawnClock(other.transform.position);
            }
        }
    }
}
