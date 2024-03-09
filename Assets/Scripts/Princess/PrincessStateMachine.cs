using UnityEngine;

public class PrincessStateMachine : MonoBehaviour
{
    #region states
    [SerializeField] private PrincessIdleProperties idleProperties;
    public PrincessIdle IdleState { get; private set; }
    [SerializeField] private PrincessRoamProperties roamProperties;
    public PrincessRoam RoamState { get; private set; }
    #endregion

    public PrincessState CurrentState { get; private set; }
    private PrincessState startingState;



    private void Awake()
    {
        IdleState = new PrincessIdle(idleProperties);
        RoamState = new PrincessRoam(roamProperties);
        startingState = IdleState;
    }

    private void Start()
    {
        CurrentState = startingState;
        CurrentState.OnEnter(this);
    }

    private void Update()
    {
        CurrentState.OnUpdate(this);
    }

    public void ChangeState(PrincessState newState)
    {
        CurrentState.OnExit(this);
        CurrentState = newState;
        CurrentState.OnEnter(this);
    }
}