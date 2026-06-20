using UnityEngine;

public class NpcStatePatrol : NpcBaseState
{
    [Header("State transitions")]
    [SerializeField] private NpcBaseState _idleState;
    [SerializeField] private NpcBaseState _fallState;

    public override void CheckExitState(NpcStateManager manager)
    {
        if (!manager.Deps.EnvDetection.IsGrounded)
        {
            manager.SwitchState(_fallState);
        }
    }

    public override void EnterState(NpcStateManager manager)
    {
        
    }

    public override void ExitState(NpcStateManager manager)
    {
        
    }

    public override void PhysicsUpdateState(NpcStateManager manager)
    {
        float dir = manager.Deps.Locomotion.IsFacingRight ? 1f : -1f;
        manager.Deps.Locomotion.Move(manager.Deps.MoveData.WalkSpeed, manager.Deps.MoveData.BaseAcceleration, dir);
    }

    public override void UpdateState(NpcStateManager manager)
    {
        if (manager.Deps.EnvDetection.IsFacingHole || manager.Deps.EnvDetection.IsFacingWall)
        {
            float dir = manager.Deps.Locomotion.IsFacingRight ? -1f : 1f;
            manager.Deps.Locomotion.ChangeDirectionByInput(dir);
            manager.SwitchState(_idleState);
        }
    }
}