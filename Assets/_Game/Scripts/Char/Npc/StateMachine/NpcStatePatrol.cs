using UnityEngine;

public class NpcStatePatrol : NpcBaseState
{
    public override void CheckExitState(NpcStateManager manager)
    {
        
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
        }
    }
}