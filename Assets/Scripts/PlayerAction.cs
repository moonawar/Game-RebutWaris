using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Action Settings")]
    [SerializeField] private float radius = 0.8f;
    [SerializeField] private float stunDuration = 1.2f;
    [SerializeField] private GameObject throwDirection;

    [Header("Input Settings")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D selfCollider;

    // For debug purposes only
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, bonkRadius);
    // }

    private bool flag = false;

    private void TryBonk()
    {
        
        if (playerMovement.IsStunned) return;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider == selfCollider) continue;

            
            if (collider.TryGetComponent(out PlayerMovement player))
            {
                /*Debug.Log(collider.GetType()); */
                player.stun(stunDuration);
            }
        }
    }

    private void TryGrab()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
        foreach (Collider2D collider in colliders)
        {
            if (collider == selfCollider) continue;


            if (collider.TryGetComponent(out PlayerMovement player))
            {
                /*Debug.Log(collider.GetType()); */
                player.grab(gameObject.transform.position);
            }
        }
    }

    private void Update()
    {
        if (playerInput.BonkInput)
        {
            TryBonk();
        }

        if (playerInput.GrabInput)
        {
            TryGrab();
        }
    }

    
}
