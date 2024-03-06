using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum PlayerId {
    Player1, Player2, Player3, Player4, None
}

public class PlayerController : MonoBehaviour
{
    /* Public fields */
    public PlayerId playerId;

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

    void Update()
    {
        // Input
        Vector2 moveInput = playerInput.MoveInput;
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0);

        // Flip the player if the direction changes
        if (Mathf.Sign(move.x) != lookDirection && move.x != 0) {
            lookDirection = (int) Mathf.Sign(move.x);

            int yRot = lookDirection == 1 ? 0 : 180;
            transform.rotation = Quaternion.Euler(0, yRot, 0);
        }

        // Movement
        transform.position += move * speed * Time.deltaTime;
    }
}
