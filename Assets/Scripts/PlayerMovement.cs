using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerId
{
    Player1, Player2, Player3, Player4, None
}

public class PlayerMovement : MonoBehaviour
{
    /* Public fields */
    [HideInInspector] public PlayerId playerId;
    public bool IsStunned { get; private set; } = false;
    public bool IsGrabbed { get; set; } = false;
    public bool grabMode { get; set; } = false;
    private Vector2 grabbedPos = Vector2.zero;

    /* Inspector fields */
    [Header("Player Settings")]
    [SerializeField] private Transform arrow;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float grabbedSpeed = 5f;
    public float CurrentSpeed { get; private set; } = 0;
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

    /* Inputs */
    private Vector2 moveInput;

    private void Awake()
    {
        lookDirection = transform.rotation.y == 0 ? RIGHT : LEFT;
        acceleration = maxSpeed / timeToAccelerate;
        deceleration = maxSpeed / timeToDecelerate;
    }

    #region stun
    public void stun(float stunDuration)
    {
        StartCoroutine(StunRoutine(stunDuration));
    }

    private IEnumerator StunRoutine(float stunDuration)
    {
        IsStunned = true;
        yield return new WaitForSeconds(stunDuration);
        IsStunned = false;
    }

    public void grab(Vector2 destination)
    {
        IsGrabbed = true;
        IsStunned = true;
        gameObject.transform.position = destination;
        //grabbedPos = destination;
    }
    public void ungrab()
    {
        IsGrabbed = false;
        IsStunned = false;
        //grabbedPos = destination;
    }
    private void MoveToGrabber(Vector2 destination)
    {
        Vector2 direction = destination - (Vector2)gameObject.transform.position;
        gameObject.transform.position += grabbedSpeed * Time.deltaTime * (Vector3)direction.normalized;
    }

    #endregion

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
            transform.position += distance * Time.deltaTime * (Vector3)direction / duration;
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

        if (IsStunned)
        {
            Debug.Log("I have been stunned!");
            return;
        }

        
        // Input
        previousMove = move;
        move = new(moveInput.x, moveInput.y, 0);
        
        if (grabMode)
        {
            print("The dir");
            print("x:" + move.x);
            print("y: " + move.y);
            //print("Masuk");
            int xDir = 0;
            int yDir = 0;
            

            if (move.y == 1)
            {
                yDir = 90;
                if(move.x != 0)
                {
                    //print("bareng");

                    yDir = 45;
                }
            }
            else if (move.y == -1)
            {
                yDir = -90;
                if (move.x != 0)
                {
                    //print("bareng");
                    yDir = -45;
                }
            }

            if (move.x == -1)
            {
                xDir = 180;
                yDir *= -1;
            }


            arrow.rotation = Quaternion.Euler(0f, 0f, xDir+yDir);
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
        if (grabMode) return;

        bool shouldDecelerate = move.magnitude == 0;

        if (shouldDecelerate)
        {
            CurrentSpeed -= deceleration * Time.fixedDeltaTime;
        }
        else
        {
            CurrentSpeed += acceleration * Time.fixedDeltaTime;
        }

        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, maxSpeed);

        // If the input is 0, use the buffer to maintain the last movement until current speed is 0
        Vector3 appliedMove = shouldDecelerate ? bufferMove : move;
        transform.position += CurrentSpeed * Time.fixedDeltaTime * appliedMove;
    }
}