using UnityEngine;

[System.Serializable]
public class PrincessRoamProperties
{
    public float RoamWalkSpeed;
    public Collider2D RoamArea;
}

public class PrincessRoam : PrincessState
{
    private readonly PrincessRoamProperties props;
    private Vector2 destination;

    public PrincessRoam(PrincessRoamProperties properties)
    {
        this.props = properties;
    }

    private Vector2 GetRandomDestination()
    {
        Vector2 randomDest = new Vector2(
            Random.Range(props.RoamArea.bounds.min.x, props.RoamArea.bounds.max.x),
            Random.Range(props.RoamArea.bounds.min.y, props.RoamArea.bounds.max.y)
        );

        return randomDest;
    }

    public override void OnEnter(PrincessStateMachine princess)
    {
        destination = GetRandomDestination();
        float yRot = destination.x > princess.transform.position.x ? 0 : 180;
        princess.transform.rotation = Quaternion.Euler(0, yRot, 0);
    }


    public override void OnUpdate(PrincessStateMachine princess)
    {
        if (Vector2.Distance(princess.transform.position, destination) < 0.1f)
        {
            princess.ChangeState(princess.IdleState);
        }

        Vector2 direction = destination - (Vector2)princess.transform.position;
        princess.transform.position += props.RoamWalkSpeed * Time.deltaTime * (Vector3)direction.normalized;
    }
}