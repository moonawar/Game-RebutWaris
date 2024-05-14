using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaketNyamuk : PowerUp
{
    public float duration;

    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        target.stun(duration);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }
}
