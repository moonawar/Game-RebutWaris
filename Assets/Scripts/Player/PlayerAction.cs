using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [Header("Action Settings")]
    [SerializeField] private float radius = 0.8f;
    [SerializeField] private float stunDuration = 1.2f;
    [SerializeField] private GameObject throwDirection;

    [Header("Input Settings")]
    [SerializeField] private GamePlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D selfCollider;

}
