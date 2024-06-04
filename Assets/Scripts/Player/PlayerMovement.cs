using System;
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
    private float prevAngle = 0;
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
    private Vector2 aimInput;

    /* Movement Transformer */
    public delegate Vector3 MovementTransformer(Vector3 move);
    public List<MovementTransformer> transformers = new List<MovementTransformer>();

    private Animator animator;
    [SerializeField] private GameObject stunnedEffect;
    #endregion

    private void Awake()
    {
        lookDirection = transform.rotation.y == 0 ? RIGHT : LEFT;
        acceleration = maxSpeed / timeToAccelerate;
        deceleration = maxSpeed / timeToDecelerate;

        animator = GetComponent<Animator>();
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
        AudioManager.Instance.PlaySFX("Stunned");
        StartCoroutine(StunRoutine(stunDuration));
    }

    private IEnumerator StunRoutine(float stunDuration)
    {
        IsStunned = true;
        stunnedEffect.SetActive(true);
        stunnedEffect.GetComponent<Animator>().SetTrigger("Stunned");
        animator.SetBool("isStunned", true);
        yield return new WaitForSeconds(stunDuration);
        IsStunned = false;
        animator.SetBool("isStunned", false);
        stunnedEffect.SetActive(false);
    }
    #endregion

    #region grab
    public void Grabbed(Vector2 destination)
    {
        IsGrabbed = true;
        IsStunned = true;

        animator.SetBool("isGrabbed", true);
        stunnedEffect.transform.localPosition += new Vector3(0, 11, 0);
        transform.position = destination + new Vector2(0, 0.1f);
    }

    private IEnumerator Thrown(float seconds)
    {
        IsThrown = true;
        stunnedEffect.transform.localPosition -= new Vector3(0, 11, 0);
        yield return new WaitForSeconds(seconds);
        CurrentSpeed = 0;
        IsThrown = false;
    }
    public void Ungrab(float theta, int flip)
    {
        IsGrabbed = false;

        animator.SetBool("isGrabbed", false);
        transform.position += new Vector3(0, 0.1f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        IsStunned = false;
        thrownAngle = new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad), 0);
        thrownAngle.x *= flip;

        lookDirection = Math.Sign(thrownAngle.x);

        CurrentSpeed = GetComponent<PlayerGrab>().initialThrowSpeed;
        StartCoroutine(Thrown(1));

    }

    public void CancelGrab()
    {
        IsGrabbed = false;

        animator.SetBool("isGrabbed", false);
        transform.position += new Vector3(0, 0.1f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
        stunnedEffect.SetActive(true);
        animator.SetBool("isStunned", true);
        yield return new WaitForSeconds(stunDuration);
        stunnedEffect.SetActive(false);
        animator.SetBool("isStunned", false);
        IsStunned = false;
    }
    #endregion

    #region input
    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
    }
    #endregion

    public Vector2 getMousePosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        if (GameplayManager.Instance.Paused) return;
        if (IsStunned && IsThrown) return;
        if(IsGrabbed) { IsStunned = true; }
        // Set current speed of animator
        animator.SetFloat("Speed", CurrentSpeed);
        
        // Input
        previousMove = move;
        print("X: " + moveInput.x);
        print("Y: " + moveInput.y);
        move = new(moveInput.x, moveInput.y, 0);
        
        if (GrabMode)
        {
            arrow.RotateAround(transform.position, Vector3.forward, 100f * Time.deltaTime);
            // Flip the player if the direction changes based on the arrow
            // float offsetCorrection = lookDirection == RIGHT ? 0 : 180;
            // float zRotation = Mathf.Abs(arrow.rotation.eulerAngles.z - offsetCorrection - 100f * Time.deltaTime);
                
            // if (zRotation > 90 && zRotation <= 270)
            // {
            //     lookDirection = LEFT;
            //     transform.rotation = Quaternion.Euler(0, 180, 0);
            // }
            // else
            // {
            //     if (zRotation >= 360) arrow.rotation *= Quaternion.Euler(0, 0, -360);

            //     lookDirection = RIGHT;
            //     transform.rotation = Quaternion.Euler(0, 0, 0);
            // }

            return;
        }

        

        if (previousMove.magnitude > 0 && move.magnitude == 0)
        {
            bufferMove = previousMove;
        }

        // Flip the player if the direction changes
        if (move.x != 0 && !IsStunned && !IsThrown && !GrabMode)
        {
            lookDirection = (int)Mathf.Sign(move.x);

            int yRot = lookDirection == 1 ? 0 : 180;
            transform.rotation = Quaternion.Euler(0, yRot, 0);
        }

        if (AimMode)
        {
            float angle;
            if (GetComponent<PlayerInput>().currentControlScheme == "Joystick" || GetComponent<PlayerInput>().currentControlScheme == "Gamepad")
            {
                if(aimInput.x == 0 && aimInput.y == 0)
                {
                    angle = prevAngle;
                }
                else
                {
                    angle = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg;
                }
                prevAngle = angle;
            }
            else
            {
                Vector2 mousePos = getMousePosition();
                Vector2 lookDir = mousePos - new Vector2(arrow.position.x, arrow.position.y);

                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            }
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            //return;
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

            // if(appliedMove.magnitude > 0)
            // {
            //     animator.SetTrigger("Move");
            // }
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
            Stun(GetComponent<PlayerBonk>().getStunDuration());
            CurrentSpeed = 0;
        }

        transform.position = new Vector2(newX, newY);
    }
}