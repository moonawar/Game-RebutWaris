using System.Collections;
using UnityEngine;

[System.Serializable]
public class EmakIdleProperties
{
    public Range IdleDuration;
}

public class EmakIdle : EmakState
{
    private readonly EmakIdleProperties properties;

    public EmakIdle(EmakIdleProperties properties)
    {
        this.properties = properties;
    }

    public override void OnEnter(EmakStateMachine princess)
    {
        princess.StartCoroutine(Idle(princess));
    }

    public override void OnExit(EmakStateMachine princess)
    {
        princess.StopAllCoroutines();
    }

    private IEnumerator Idle(EmakStateMachine princess)
    {
        yield return new WaitForSeconds(properties.IdleDuration.RandomValue());
        princess.ChangeState(princess.RoamState);
    }
}