using UnityEngine;

public class Goose : MonoBehaviour
{
    private readonly float stunDuration = 2;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerBody")) {
            other.GetComponentInParent<PlayerMovement>().Stun(stunDuration);
            if (other.GetComponentInParent<PlayerMash>().HaveClock)
            {
                other.GetComponentInParent<PlayerMash>().HaveClock = false;
                ClockManager.Instance.SpawnClock(other.transform.position);
            }
        }
    }
}
