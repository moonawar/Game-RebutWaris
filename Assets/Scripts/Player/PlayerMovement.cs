using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region restriction states
    public bool isImmune { get; set; } = false;
    public bool IsStunned { get; private set; } = false;
    public bool IsGrabbed { get; set; } = false;
    public bool GrabMode { get; set; } = false;
    private bool isThrown = false;
    private Vector3 thrownAngle;
    #endregion

    #region movement
    /* Inspector fields */
    [Header("Player Settings")]
    [SerializeField] private Transform arrow;
    [SerializeField] private float maxSpeed = 5f;
    public float CurrentSpeed { get; private set; } = 0;
    [SerializeField] private float timeToAccelerate = 0.2f;
    private float acceleration;
    [SerializeField] private float timeToDecelerate = 0.12f;
    private float deceleration;

    /* Private fields */
    public static int LEFT = -1;
    public static int RIGHT = 1;
    public int lookDirection;
    private Vector3 move; // Store the move on the current frame
    private Vector3 previousMove; // Store the move on the previous frame
    private Vector3 bufferMove; // Store the last move before the input went to 0

    /* Inputs */
    private Vector2 moveInput;

    /* Movement Transformer */
    public delegate Vector3 MovementTransformer(Vector3 move);
    public List<MovementTransformer> transformers = new List<MovementTransformer>();
    #endregion

    private void Awake()
    {
        lookDirection = transform.rotation.y == 0 ? RIGHT : LEFT;
        acceleration = maxSpeed / timeToAccelerate;
        deceleration = maxSpeed / timeToDecelerate;
    }

    #region stun
    public void Stun(float stunDuration)
    {
        if (isImmune) return;
        StartCoroutine(StunRoutine(stunDuration));
    }

    private IEnumerator StunRoutine(float stunDuration)
    {
        IsStunned = true;
        yield return new WaitForSeconds(stunDuration);
        IsStunned = false;
    }
    #endregion

    #region grab
    public void Grab(Vector2 destination)
    {
        print(gameObject.name + "is grabbed!");
        IsGrabbed = true;
        IsStunned = true;

        if(Mathf.Abs(transform.eulerAngles.y) != 180)
        {
            gameObject.transform.position = destination + new Vector2(0.5f, 1.5f);
        }
        else
        {
            gameObject.transform.position = destination + new Vector2(-0.5f, 1.5f);
        }
        gameObject.transform.eulerAngles += new Vector3(0, 0, -90);
    }
    public void Ungrab(float theta, int flip)
    {
        IsGrabbed = false;
        isThrown = true;

        if (Mathf.Abs(transform.eulerAngles.y) != 180)
        {
            gameObject.transform.position += new Vector3(-0.5f, -1.5f);

        }
        else
        {
            gameObject.transform.position += new Vector3(0.5f, -1.5f);
        }
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        IsStunned = false;
        thrownAngle = new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad), 0);
        thrownAngle.x *= flip;

        print("is thrown in x:" + thrownAngle.x + " y: " + thrownAngle.y);
        CurrentSpeed = 50;
    }
    #endregion

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
            transform.position += distance * Time.deltaTime * (Vector3)direction / duration;
            timer += Time.deltaTime;
            yield return null;
        }
        IsStunned = true;
        yield return new WaitForSeconds(stunDuration);
        IsStunned = false;
    }
    #endregion

    #region input
    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    #endregion

    void Update()
    {
        if (IsStunned)
        {
            // Can't do any movement
            return;
        }
        
        // Input
        previousMove = move;
        move = new(moveInput.x, moveInput.y, 0);
        
        if (GrabMode)
        {
            arrow.RotateAround(transform.position, Vector3.forward, 1f);
            return;
        }

        if (previousMove.magnitude > 0 && move.magnitude == 0)
        {
            bufferMove = previousMove;
        }

        // Flip the player if the direction changes
        if (Mathf.Sign(move.x) != lookDirection && move.x != 0 && !IsGrabbed)
        {
            lookDirection = (int)Mathf.Sign(move.x);

            int yRot = lookDirection == 1 ? 0 : 180;
            transform.rotation = Quaternion.Euler(0, yRot, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("hit");
        CurrentSpeed = 0;
    }

    private void FixedUpdate()
    {
        if (IsStunned) return; // Do nothing
        if (GrabMode) return;

        bool shouldDecelerate = move.magnitude == 0;

        if (shouldDecelerate && CurrentSpeed >=0)
        {
            CurrentSpeed -= deceleration * Time.fixedDeltaTime;
        }
        else
        {
            CurrentSpeed += acceleration * Time.fixedDeltaTime;
        }

        if (isThrown)
        {
            transform.position += CurrentSpeed * Time.fixedDeltaTime * thrownAngle;
        }
        else
        {
            CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, maxSpeed);

            // If the input is 0, use the buffer to maintain the last movement until current speed is 0
            Vector3 appliedMove = (shouldDecelerate ? bufferMove : move) * CurrentSpeed;
            // Apply transformers
            foreach (MovementTransformer transformer in transformers)
            {
                appliedMove = transformer(appliedMove);
            }
            transform.position += Time.fixedDeltaTime * appliedMove;
        }

        if (isThrown && CurrentSpeed <= 0)
        {
            isThrown = false;
        }

        
    }
}