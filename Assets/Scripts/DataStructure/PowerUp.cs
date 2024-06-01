using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PowerUp: MonoBehaviour 
{
    protected PlayerMovement target;
    public virtual IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        yield return null;
    }

    protected void PickedUp(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody") == false) return;
        GameObject player = collision.gameObject.GetComponentInParent<PlayerMovement>().gameObject;
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        if (playerMovement == null || playerMovement.IsGrabbed || playerMovement.IsStunned) return;

        player.GetComponent<PlayerUseItem>().EquipItem(this);
        if (player.GetComponent<PlayerInput>().playerIndex == 0)
        {
            transform.SetParent(PowerUpSpawner.Instance.PowerUpPanelP1.transform);
        }
        else
        {
            transform.SetParent(PowerUpSpawner.Instance.PowerUpPanelP2.transform);
        }
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(50f, 50f, 50f);
        GetComponent<SpriteRenderer>().sortingOrder = 2;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    protected void FindOppositeTarget()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerInput>().playerIndex != target.GetComponent<PlayerInput>().playerIndex)
            {
                target = player.GetComponent<PlayerMovement>();
                return;
            }
        }
    }
}
