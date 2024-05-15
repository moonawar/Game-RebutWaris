using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public abstract class PowerUp: MonoBehaviour 
{
    protected PlayerMovement target;
    public virtual void Activate(GameObject target) { }
    public virtual IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        yield return null;
    }

    protected void PickedUp(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() == null || collision.gameObject.GetComponent<PlayerMovement>().IsGrabbed == true || collision.gameObject.GetComponent<PlayerMovement>().IsStunned == true) return;

        collision.gameObject.GetComponent<PlayerUseItem>().EquipItem(this);
        if (collision.gameObject.GetComponent<PlayerInput>().playerIndex == 0)
        {
            this.transform.SetParent(GameObject.Find("ItemPanelP1").transform);
        }
        else
        {
            this.transform.SetParent(GameObject.Find("ItemPanelP2").transform);
        }
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localScale = new Vector3(50f, 50f, 50f);

    }

    protected void FindOppositeTarget()
    {

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerInput>().playerIndex != target.GetComponent<PlayerInput>().playerIndex)
            {
                this.target = player.GetComponent<PlayerMovement>();
                return;
            }
        }
    }
}
