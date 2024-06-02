using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaketNyamuk : PowerUp
{
    public float duration;

    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        target.Stun(duration);
        yield return null;
        PowerUpEnd.Invoke(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }
}
