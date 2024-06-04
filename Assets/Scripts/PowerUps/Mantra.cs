using System.Collections;
using UnityEngine;

public class Mantra : DurationPowerUp
{

    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        AudioManager.Instance.PlaySFX("PowerUp_Mantra");
        this.target = target;
        GameObject emak = GameObject.FindGameObjectWithTag("Emak");
        EmakStateMachine emakStateMachine = emak.GetComponent<EmakStateMachine>();
        FindOppositeTarget();
        emakStateMachine.CharmedState.target = this.target;
        emakStateMachine.ChangeState(emakStateMachine.CharmedState);

        timer = duration;
        yield return new WaitForSeconds(duration);

        PowerUpEnd.Invoke(this);
        emakStateMachine.ChangeState(emakStateMachine.IdleState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }
}
