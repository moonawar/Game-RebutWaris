using UnityEngine;

public class GameClock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerBody")) {
            PlayerMash player = other.GetComponentInParent<PlayerMash>();
            player.HaveClock = true;
            Destroy(gameObject);
        }
    }
}
