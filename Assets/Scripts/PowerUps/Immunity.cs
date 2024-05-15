using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immunity : PowerUp
{
    public float duration;

    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        this.target = target;
        FindOppositeTarget();
        this.target.isImmune = true;
        yield return new WaitForSeconds(duration);
        this.target.isImmune = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }
}