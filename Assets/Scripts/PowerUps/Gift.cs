using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : DurationPowerUp
{
    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        AudioManager.Instance.PlaySFX("PowerUp_Gift");
        this.target = target;
        FindOppositeTarget();
        float prevRate = this.target.GetComponent<PlayerMash>().GetIncreaseRate();
        this.target.GetComponent<PlayerMash>().SetIncreaseRate(prevRate * 1.5f);
        timer = duration;
        yield return new WaitForSeconds(duration);

        PowerUpEnd.Invoke(this);
        this.target.GetComponent<PlayerMash>().SetIncreaseRate(prevRate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }
}
