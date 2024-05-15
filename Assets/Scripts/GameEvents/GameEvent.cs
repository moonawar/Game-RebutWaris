using UnityEngine;

public abstract class GameEvent : ScriptableObject
{
    public virtual void OnEnter(GameEventsManager mgr) {}
}
