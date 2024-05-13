using UnityEngine;

public abstract class GameEvent : ScriptableObject
{
    public virtual void Enter() {}
    public virtual void Update() {}
    public virtual void Exit() {}
}
