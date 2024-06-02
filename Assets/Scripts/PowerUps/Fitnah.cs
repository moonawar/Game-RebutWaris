using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fitnah : PowerUp
{
    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        target.GetComponent<PlayerMash>().DecreaseLoveHalf();
        yield return null;
        PowerUpEnd.Invoke(this);
        Destroy(gameObject);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }


}
