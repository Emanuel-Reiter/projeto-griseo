using UnityEngine;

public class NpcStateIdle : NpcBaseState
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
        manager.Deps.Locomotion.Decelerate(manager.Deps.MoveData.BaseAcceleration);
    }

    public override void UpdateState(NpcStateManager manager)
    {
        
    }
}