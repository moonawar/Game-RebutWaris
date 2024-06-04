using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaketNyamuk : PowerUp
{
    public float duration;

    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        AudioManager.Instance.PlaySFX("PowerUp_RaketNyamuk");
        target.Stun(duration);
        if (target.GetComponent<PlayerMash>().HaveClock)
        {
            target.GetComponent<PlayerMash>().HaveClock = false;
            ClockManager.Instance.SpawnClock(target.transform.position);
        }

        yield return null;
        PowerUpEnd.Invoke(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }
}
