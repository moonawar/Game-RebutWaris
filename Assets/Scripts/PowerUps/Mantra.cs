using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantra : PowerUp
{
    public float duration;
    
    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        this.target = target;
        GameObject emak = GameObject.FindGameObjectWithTag("Emak");
        EmakStateMachine emakStateMachine = emak.GetComponent<EmakStateMachine>();
        FindOppositeTarget();
        emakStateMachine.CharmedState.target = this.target;
        emakStateMachine.ChangeState(emakStateMachine.CharmedState);
        yield return new WaitForSeconds(duration);
        emakStateMachine.ChangeState(emakStateMachine.IdleState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }
}
