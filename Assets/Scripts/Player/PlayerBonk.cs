using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBonk : MonoBehaviour
{
    [Header("Bonk Settings")]
    [SerializeField] private float radius = 0.8f;
    [SerializeField] private float stunDuration = 1.2f;
    [SerializeField] public PlayerMovement playerMovement;
    [SerializeField] private Collider2D selfCollider;

    public void OnBonk(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print("ITS BEING PERFORMED");
        }
        if (playerMovement.IsStunned) return;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider == selfCollider) continue;


            if (collider.TryGetComponent(out PlayerMovement player))
            {
                player.Stun(stunDuration);
            }
        }
    }
}
