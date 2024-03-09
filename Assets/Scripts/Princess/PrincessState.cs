using UnityEngine;

public abstract class PrincessState {
    public virtual void OnEnter(PrincessStateMachine princess) { }
    public virtual void OnUpdate(PrincessStateMachine princess) { }
    public virtual void OnExit(PrincessStateMachine princess) { }
}