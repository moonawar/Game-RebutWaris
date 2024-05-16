using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region restriction states
    public bool isImmune { get; set; } = false;
    public bool IsStunned { get; private set; } = false;
    public bool IsGrabbed { get; set; } = false;
    public bool GrabMode { get; set; } = false;
    public bool AimMode { get; set; } = false;
    private bool IsThrown = false;
    private Vector3 thrownAngle;
    #endregion

    #region movement
    /* Inspector fields */
    [Header("Player Settings")]
    [SerializeField] private Transform arrow;
    [SerializeField] private float maxSpeed = 5f;
    private Collider2D arena;
    private Camera cam;
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

    public void SetArena(Collider2D collider)
    {
        arena = collider;
    }

    public Collider2D GetArena()
    {
        return arena;
    }

    public void SetCamera(Camera cam)
    {
        this.cam  = cam;
    }

    public void SetArrow(Sprite sprite)
    {
        arrow.GetComponentInChildren<Image>().sprite = sprite;

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

    private IEnumerator Thrown(float seconds)
    {
        IsThrown = true;
        yield return new WaitForSeconds(seconds);
        IsThrown = false;
    }
    public void Ungrab(float theta, int flip)
    {
        IsGrabbed = false;
        IsStunned = false;

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

        CurrentSpeed = 50;
        StartCoroutine(Thrown(1));

    }
    #endregion

    #region throwItem

    public void ThrowItem(Vector3 direction)
    {

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

    public Vector2 getMousePosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        if (IsStunned && IsThrown) return;
        
        // Input
        previousMove = move;
        move = new(moveInput.x, moveInput.y, 0);
        
        if (GrabMode)
        {
            arrow.RotateAround(transform.position, Vector3.forward, 1f);
            return;
        }

        if (AimMode)
        {
            Vector2 mousePos = getMousePosition();
            Vector2 lookDir = mousePos - new Vector2(arrow.position.x, arrow.position.y);
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0,0,angle);
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

        if (IsThrown)
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


        // Stay in bounds
        StayInBounds();
    }

    private void StayInBounds()
    {
        float newX = transform.position.x;
        float newY = transform.position.y;
        bool hitWall = false;
        if (transform.position.x <= arena.bounds.min.x)
        {
            newX = arena.bounds.min.x;
            hitWall = true;
        }
        else if (transform.position.x >= arena.bounds.max.x)
        {
            newX = arena.bounds.max.x;
            hitWall = true;
        }

        if (transform.position.y <= arena.bounds.min.y)
        {
            newY = arena.bounds.min.y;
            hitWall = true;
        }
        else if (transform.position.y >= arena.bounds.max.y)
        {
            newY = arena.bounds.max.y;
            hitWall = true;
        }

        if (IsThrown && hitWall)
        {
            IsThrown = false;
        }

        transform.position = new Vector2(newX, newY);
    }
}