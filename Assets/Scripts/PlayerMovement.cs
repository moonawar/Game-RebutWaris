using UnityEngine;
using System.Collections;

public enum PlayerId {
    Player1, Player2, Player3, Player4, None
}

public class PlayerMovement : MonoBehaviour
{
    /* Public fields */
    public PlayerId playerId;
    public bool IsStunned {get; private set;} = false;

    /* Inspector fields */
    [Header("Input Settings")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Player Settings")]
    [SerializeField] private float speed = 5f;
    
    /* Private fields */
    private const int LEFT = -1;
    private const int RIGHT = 1;
    private int lookDirection;

    private void Awake()
    {
        lookDirection = transform.rotation.y == 0 ? RIGHT : LEFT;
    }

    #region knockback
    public void Knockback(Vector2 direction, float distance, float duration, float stunDuration)
    {
        StartCoroutine(KnockbackRoutine(direction, distance, duration, stunDuration));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float distance, float duration, float stunDuration)
    {
        float timer = 0;
        while (timer < duration)
        {
            transform.position += distance * Time.deltaTime * (Vector3) direction / duration;
            timer += Time.deltaTime;
            yield return null;
        }
        IsStunned = true;
        yield return new WaitForSeconds(stunDuration);
        IsStunned = false;
    }
    #endregion

    void Update()
    {
        if (IsStunned) return; // Do nothing

        // Input
        Vector2 moveInput = playerInput.MoveInput;
        Vector3 move = new(moveInput.x, moveInput.y, 0);

        // Flip the player if the direction changes
        if (Mathf.Sign(move.x) != lookDirection && move.x != 0) {
            lookDirection = (int) Mathf.Sign(move.x);

            int yRot = lookDirection == 1 ? 0 : 180;
            transform.rotation = Quaternion.Euler(0, yRot, 0);
        }

        // Movement
        transform.position += speed * Time.deltaTime * move;
    }
}
