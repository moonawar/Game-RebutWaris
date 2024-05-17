using UnityEngine;

public class GameClock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerBody")) {
            PlayerMash player = other.GetComponentInParent<PlayerMash>();
            Debug.Log("Player picked up the clock");
            player.HaveClock = true;
            Destroy(gameObject);
        }
    }
}
