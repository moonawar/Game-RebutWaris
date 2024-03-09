using System.Collections;
using UnityEngine;

[System.Serializable]
public class PrincessIdleProperties
{
    public Range IdleDuration;
}

public class PrincessIdle : PrincessState
{
    private readonly PrincessIdleProperties properties;

    public PrincessIdle(PrincessIdleProperties properties)
    {
        this.properties = properties;
    }

    public override void OnEnter(PrincessStateMachine princess)
    {
        princess.StartCoroutine(Idle(princess));
    }

    private IEnumerator Idle(PrincessStateMachine princess)
    {
        yield return new WaitForSeconds(properties.IdleDuration.RandomValue());
        princess.ChangeState(princess.RoamState);
    }
}