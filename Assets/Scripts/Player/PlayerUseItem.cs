using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUseItem : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D selfCollider;
    [SerializeField] private PowerUp powerUp;

    public void EquipItem(PowerUp pickedUp)
    {
        if(powerUp != null)
        {
            Destroy(powerUp.gameObject);
        }
        powerUp = pickedUp;
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed && powerUp != null)
        {
            print("ITS BEING PERFORMED");

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 100);
            foreach (Collider2D collider in colliders)
            {
                if (collider != selfCollider && collider.TryGetComponent(out PlayerMovement player))
                {
                    StartCoroutine(powerUp.PowerUpCoroutine(player));
                }
                
            }

            Destroy(powerUp.gameObject);
            powerUp = null;

        }

    }
}
