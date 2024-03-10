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
    [SerializeField] private float maxSpeed = 5f;
    public float CurrentSpeed {get; private set;} = 0;
    [SerializeField] private float timeToAccelerate = 0.2f;
    private float acceleration;
    [SerializeField] private float timeToDecelerate = 0.12f;
    private float deceleration;

    
    /* Private fields */
    private const int LEFT = -1;
    private const int RIGHT = 1;
    private int lookDirection;
    private Vector3 move; // Store the move on the current frame
    private Vector3 previousMove; // Store the move on the previous frame
    private Vector3 bufferMove; // Store the last move before the input went to 0

    private void Awake()
    {
        lookDirection = transform.rotation.y == 0 ? RIGHT : LEFT;
        acceleration = maxSpeed / timeToAccelerate;
        deceleration = maxSpeed / timeToDecelerate;
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
        previousMove = move;
        move = new(moveInput.x, moveInput.y, 0);

        if (previousMove.magnitude > 0 && move.magnitude == 0) {
            bufferMove = previousMove;
        }

        // Flip the player if the direction changes
        if (Mathf.Sign(move.x) != lookDirection && move.x != 0) {
            lookDirection = (int) Mathf.Sign(move.x);

            int yRot = lookDirection == 1 ? 0 : 180;
            transform.rotation = Quaternion.Euler(0, yRot, 0);
        }
    }

    private void FixedUpdate() {
        if (IsStunned) return; // Do nothing

        bool shouldDecelerate = move.magnitude == 0;

        if (shouldDecelerate) {
            CurrentSpeed -= deceleration * Time.fixedDeltaTime;
        } else {
            CurrentSpeed += acceleration * Time.fixedDeltaTime;
        }

        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, maxSpeed);

        // If the input is 0, use the buffer to maintain the last movement until current speed is 0
        Vector3 appliedMove = shouldDecelerate ? bufferMove : move; 
        transform.position += CurrentSpeed * Time.fixedDeltaTime * appliedMove;
    }
}