using UnityEngine;

public class PlayerBonk : MonoBehaviour
{
    [Header("Bonk Settings")]
    [SerializeField] private float bonkRadius = 0.8f;
    [SerializeField] private float knockbackDistance = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float stunDuration = 1.2f;

    [Header("Input Settings")]
    [SerializeField] private GamePlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D selfCollider;

    // For debug purposes only
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, bonkRadius);
    // }

    private void TryBonk() {
        if (playerMovement.IsStunned) return;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bonkRadius);
        foreach (Collider2D collider in colliders) {
            if (collider == selfCollider) continue;
            if (collider.TryGetComponent(out PlayerMovement player)) {
                Vector2 knockbackDirection = (player.transform.position - selfCollider.transform.position).normalized;
                player.Knockback(knockbackDirection, knockbackDistance, knockbackDuration, stunDuration);
            }
        }
    }

    private void Update() {
        if (playerInput.BonkInput) {
            TryBonk();
        }
    }
}
