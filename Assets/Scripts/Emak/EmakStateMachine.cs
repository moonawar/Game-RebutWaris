using UnityEngine;

public class EmakStateMachine : MonoBehaviour
{
    #region states
    [SerializeField] private EmakIdleProperties idleProperties;
    public EmakIdle IdleState { get; private set; }
    [SerializeField] private EmakRoamProperties roamProperties;
    public EmakRoam RoamState { get; private set; }
    [SerializeField] private EmakCharmedProperties charmedProperties;
    public EmakCharmed CharmedState { get; private set; }
    #endregion

    public EmakState CurrentState { get; private set; }
    private EmakState startingState;
    private ParticleSystem _charmedParticles;
    public Animator animator { get; private set;}

    private void Awake()
    {
        IdleState = new EmakIdle(idleProperties);
        RoamState = new EmakRoam(roamProperties);
        CharmedState = new EmakCharmed(charmedProperties, null);
        startingState = IdleState;
        _charmedParticles = GetComponentInChildren<ParticleSystem>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CurrentState = startingState;
        CurrentState.OnEnter(this);
    }

    private void Update()
    {
        if (GameplayManager.Instance.Paused) return;
        CurrentState.OnUpdate(this);
    }

    public void ChangeState(EmakState newState)
    {
        CurrentState.OnExit(this);
        CurrentState = newState;
        CurrentState.OnEnter(this);
    }

    public void EmitLoveParticles()
    {
        _charmedParticles.Play();
    }
}