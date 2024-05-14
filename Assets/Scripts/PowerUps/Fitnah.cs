using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fitnah : PowerUp
{
    public override void Activate(GameObject target)
    {
        target.GetComponent<EmakRadar>().DecreaseLoveHalf();
    }

    public override IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        target.GetComponent<EmakRadar>().DecreaseLoveHalf();
        yield return null;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickedUp(collision);
    }


}
