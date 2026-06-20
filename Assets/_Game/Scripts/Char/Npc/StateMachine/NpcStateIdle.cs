using UnityEngine;

public class NpcStateIdle : NpcBaseState
{
    [Header("State transitions")]
    [SerializeField] private NpcBaseState _patrolState;
    [SerializeField] private NpcBaseState _fallState;

    private bool _wasAirborne = false;

    public override void CheckExitState(NpcStateManager manager)
    {
        if (!manager.Deps.EnvDetection.IsGrounded)
        {
            manager.SwitchState(_fallState);
        }

        if (!_wasAirborne && GetCurrentTime() < GetStateDuration()) return;

        manager.SwitchState(_patrolState);
        return;
    }

    public override void EnterState(NpcStateManager manager)
    {
        _wasAirborne = manager.WasPreviousState<NpcStateFall>();
    }

    public override void ExitState(NpcStateManager manager)
    {
        
    }

    public override void PhysicsUpdateState(NpcStateManager manager)
    {
        manager.Deps.Locomotion.Decelerate(manager.Deps.MoveData.BaseAcceleration);
    }

    public override void UpdateState(NpcStateManager manager)
    {
        
    }
}