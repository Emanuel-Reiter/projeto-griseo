using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    // State management
    [SerializeField] private PlayerBaseState _initialState;
    public PlayerBaseState CurrentState { get; private set; }
    public PlayerBaseState PreviousState { get; private set; }

    private PlayerDependencies _dependencies;
    public PlayerDependencies Deps => _dependencies;

    private void Awake()
    {
        _dependencies = GetComponent<PlayerDependencies>();
    }

    private void Start()
    {
        // Set the initial state
        SwitchState(_initialState);

        // Set player gravity scale
        Deps.Locomotion.SetGravityModifier(Deps.MoveData.GravityMultiplaier);
    }

    private void Update()
    {
        CurrentState?.CheckExitState(this);
        CurrentState?.UpdateState(this);
    }

    private void FixedUpdate()
    {
        CurrentState?.PhysicsUpdateState(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        if (newState == null) return;

        CurrentState?.ExitState(this);
        PreviousState = CurrentState;

        CurrentState = newState;
        CurrentState.InitializeState();
        CurrentState?.EnterState(this);
        PlayBaseAnim(newState.GetBaseAnim());
    }

    private void PlayBaseAnim(AnimationClip clip)
    {
        if (clip == null) return;
        Deps.CharAnimator.Play(clip);
    }

    public bool WasPreviousState<T>() where T : PlayerBaseState { return PreviousState is T; }

    public bool IsCurrentState<T>() where T : PlayerBaseState { return CurrentState is T; }
}