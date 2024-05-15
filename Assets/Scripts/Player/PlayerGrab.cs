using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrab : MonoBehaviour
{
    [Header("Grab Settings")]
    [SerializeField] private float radius = 4;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D selfCollider;
    [SerializeField] private Transform arrow;

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print("ITS BEING PERFORMED");

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in colliders)
            {
                if (collider != selfCollider && collider.TryGetComponent(out PlayerMovement player))
                {
                    player.Grab(gameObject.transform.position);
                    arrow.gameObject.SetActive(true);
                    playerMovement.GrabMode = true;
                    playerMovement.GetComponent<Animator>().SetTrigger("Grab");
                }
            }

        }

        if(context.canceled)
        {
            print("ITS FINISHED");

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in colliders)
            {
                if (collider == selfCollider) continue;


                if (collider.TryGetComponent(out PlayerMovement player))
                {
                    playerMovement.GrabMode = false;
                    arrow.RotateAround(transform.position, Vector3.forward, 0f);
                    if(Mathf.Abs(transform.rotation.eulerAngles.y) == 180)
                    {
                        player.Ungrab(arrow.transform.rotation.eulerAngles.z, -1);

                    }
                    else
                    {
                        player.Ungrab(arrow.transform.rotation.eulerAngles.z, 1);

                    }
                    playerMovement.GetComponent<Animator>().SetTrigger("Throw");

                    arrow.gameObject.SetActive(false);
                }
            }

        }
        
    }
}
