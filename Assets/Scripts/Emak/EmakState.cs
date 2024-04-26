public abstract class EmakState {
    public virtual void OnEnter(EmakStateMachine princess) { }
    public virtual void OnUpdate(EmakStateMachine princess) { }
    public virtual void OnExit(EmakStateMachine princess) { }
}