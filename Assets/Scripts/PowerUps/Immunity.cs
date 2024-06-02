using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immunity : DurationPowerUp
{
    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        this.target = target;
        FindOppositeTarget();
        this.target.isImmune = true;
        timer = duration;
        yield return new WaitForSeconds(duration);
        PowerUpEnd.Invoke(this);
        this.target.isImmune = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }
}
