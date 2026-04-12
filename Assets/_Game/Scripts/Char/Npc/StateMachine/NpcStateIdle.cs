using UnityEngine;

public class NpcStateIdle : NpcBaseState
{
    [Header("State transitions")]
    [SerializeField] private NpcBaseState _patrolState;

    public override void CheckExitState(NpcStateManager manager)
    {
        if (GetCurrentTime() < GetStateDuration()) return;

        manager.SwitchState(_patrolState);
        return;
    }

    public override void EnterState(NpcStateManager manager)
    {

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