using System.Diagnostics;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[System.Serializable]
public class EmakCharmedProperties
{
    public float WalkSpeed;
}

public class EmakCharmed : EmakState
{
    private readonly EmakCharmedProperties props;
    private Vector2 destination;
    public PlayerMovement target;

    public EmakCharmed(EmakCharmedProperties properties, PlayerMovement target)
    {
        this.props = properties;
        this.target = target;
    }

    public override void OnUpdate(EmakStateMachine emak)
    {
        destination = target.transform.position;
        float yRot = destination.x > emak.transform.position.x ? 0 : 180;
        emak.transform.rotation = Quaternion.Euler(0, yRot, 0);


        if (Vector2.Distance(emak.transform.position, destination) >= 0.1f)
        {
            emak.animator.SetBool("isRoaming", true);
            Vector2 direction = destination - (Vector2)emak.transform.position;
            emak.transform.position += props.WalkSpeed * Time.deltaTime * (Vector3)direction.normalized;
        }
        else
        {
            emak.animator.SetBool("isRoaming", false);
        }
    }
}