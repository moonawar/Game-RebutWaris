using UnityEngine;

[System.Serializable]
public class EmakRoamProperties
{
    public float RoamWalkSpeed;
    public Collider2D RoamArea;
}

public class EmakRoam : EmakState
{
    private readonly EmakRoamProperties props;
    private Vector2 destination;

    public EmakRoam(EmakRoamProperties properties)
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

    public override void OnEnter(EmakStateMachine princess)
    {
        destination = GetRandomDestination();
        float yRot = destination.x > princess.transform.position.x ? 0 : 180;
        princess.transform.rotation = Quaternion.Euler(0, yRot, 0);
    }


    public override void OnUpdate(EmakStateMachine princess)
    {
        if (Vector2.Distance(princess.transform.position, destination) < 0.1f)
        {
            princess.ChangeState(princess.IdleState);
        }

        Vector2 direction = destination - (Vector2)princess.transform.position;
        princess.transform.position += props.RoamWalkSpeed * Time.deltaTime * (Vector3)direction.normalized;
    }
}