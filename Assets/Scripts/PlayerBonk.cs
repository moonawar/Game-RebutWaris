using UnityEngine;

public class PlayerBonk : MonoBehaviour
{
    [Header("Bonk Settings")]
    [SerializeField] private float bonkRadius = 0.8f;
    [SerializeField] private float knockbackDistance = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    [Header("Input Settings")]
    [SerializeField] private PlayerInput playerInput;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bonkRadius);
    }

    private void TryBonk() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bonkRadius);
        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent(out PlayerMovement player)) {
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                player.Knockback(knockbackDirection, knockbackDistance, knockbackDuration);
            }
        }
    }
}
