using UnityEngine;

public abstract class GameEvent : ScriptableObject
{
    public abstract string GetName();
    public virtual void OnEnter(GameEventsManager mgr) {}
}
