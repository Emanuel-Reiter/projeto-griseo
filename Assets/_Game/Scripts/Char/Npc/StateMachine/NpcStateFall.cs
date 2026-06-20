using UnityEngine;

public class NpcStateFall : NpcBaseState
{
    [Header("State transitions")]
    [SerializeField] private NpcBaseState _idleState;

    public override void CheckExitState(NpcStateManager manager)
    {
        if (!manager.Deps.EnvDetection.IsGrounded) return;

        manager.SwitchState(_idleState);
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