using UnityEngine;

public class NpcStateManager : MonoBehaviour
{
    // State management
    [SerializeField] private NpcBaseState _initialState;
    public NpcBaseState CurrentState { get; private set; }
    public NpcBaseState PreviousState { get; private set; }

    private NpcDependencies _dependencies;
    public NpcDependencies Deps => _dependencies;

    [SerializeField] private bool _isActive = true;
    public bool IsActive => _isActive;

    private void Awake()
    {
        _dependencies = GetComponent<NpcDependencies>();
    }

    private void Start()
    {
        // Set the initial state
        SwitchState(_initialState);

        // Set Npc gravity scale
        Deps.Locomotion.SetGravityModifier(Deps.MoveData.GravityMultiplaier);
    }

    private void Update()
    {
        if (!_isActive) return;

        CurrentState?.CheckExitState(this);
        CurrentState?.UpdateState(this);
    }

    private void FixedUpdate()
    {
        if (!_isActive) return;

        CurrentState?.PhysicsUpdateState(this);
    }

    public void SwitchState(NpcBaseState newState)
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

    public bool WasPreviousState<T>() where T : NpcBaseState { return PreviousState is T; }

    public bool IsCurrentState<T>() where T : NpcBaseState { return CurrentState is T; }

    public void ToggleNpc(bool toggle)
    {
        _isActive = toggle;
    }
}