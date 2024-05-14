using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantra : PowerUp
{
    public float duration;
    
    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        print("Emak is going to be charmed");
        this.target = target;
        print("Current target: " + this.target.playerId);

        FindOppositeTarget();
        print("New target: " + this.target.playerId);

        GameObject.FindGameObjectWithTag("Emak").GetComponent<EmakStateMachine>().CharmedState.target = this.target;
        GameObject.FindGameObjectWithTag("Emak").GetComponent<EmakStateMachine>().ChangeState(GameObject.FindGameObjectWithTag("Emak").GetComponent<EmakStateMachine>().CharmedState);
        print("Emak is charmed");
        yield return new WaitForSeconds(duration);
        GameObject.FindGameObjectWithTag("Emak").GetComponent<EmakStateMachine>().ChangeState(GameObject.FindGameObjectWithTag("Emak").GetComponent<EmakStateMachine>().IdleState);
        print("Emak is no longer charmed");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }
}
