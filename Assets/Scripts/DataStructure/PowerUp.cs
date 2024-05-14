using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
        if (collision.gameObject.GetComponent<PlayerMovement>() == null) return;

        collision.gameObject.GetComponent<PlayerUseItem>().EquipItem(this);
        if (collision.gameObject.GetComponent<PlayerMovement>().playerId == PlayerId.Player1)
        {
            this.transform.SetParent(GameObject.Find("ItemPanelP1").transform);
        }
        else
        {
            this.transform.SetParent(GameObject.Find("ItemPanelP2").transform);
        }
        this.transform.localPosition = new Vector3(0, 0, 0);

    }

    protected void FindOppositeTarget()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            print(player.GetComponent<PlayerMovement>().playerId);
            if (!(player.GetComponent<PlayerMovement>().playerId.Equals(target.GetComponent<PlayerMovement>().playerId)))
            {
                print("New target: " + player.GetComponent<PlayerMovement>().playerId);
                this.target = player.GetComponent<PlayerMovement>();
                return;
            }
        }
    }
}
