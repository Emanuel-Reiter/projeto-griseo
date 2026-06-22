using UnityEngine;

public class NpcStateChase : NpcBaseState
{
    [Header("State transitions")]
    [SerializeField] private NpcBaseState _idleState;
    [SerializeField] private NpcBaseState _fallState;
    [SerializeField] private NpcBaseState _atkState;

    [Header("Other params")]
    [SerializeField] private float _minAttackDsistance = 10f;

    public override void CheckExitState(NpcStateManager manager)
    {
        if (!manager.Deps.TargetDetection.HasTarget)
        {
            manager.SwitchState(_idleState);
            return;
        }

        if (!manager.Deps.EnvDetection.IsGrounded)
        {
            manager.SwitchState(_fallState);
            return;
        }

        if (manager.Deps.TargetDetection.GetDistanceFromTarget() < _minAttackDsistance)
        {
            manager.SwitchState(_atkState);
            return;
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
        else
        {
            float xDiff = manager.Deps.TargetDetection.TargetRef.position.x - transform.position.x;
            if (xDiff > 0f)
            {
                manager.Deps.Locomotion.ChangeDirectionByInput(1f);
            }
            else
            {
                manager.Deps.Locomotion.ChangeDirectionByInput(-1f);
            }
        }
    }
}