using UnityEngine;

public class NpcStaticStateIdle : NpcBaseState
{
    [Header("State transitions")]
    [SerializeField] private NpcBaseState _castState;

    public override void CheckExitState(NpcStateManager manager)
    {
        if (!HasCompletedExitTime()) return;

        if (manager.Deps.TargetDetection.HasTarget)
        {
            manager.SwitchState(_castState);
        }
    }

    public override void EnterState(NpcStateManager manager)
    {
        if (_baseAnim == null) return;

        SetStateDuration(_baseAnim.length);
        manager.Deps.CharAnimator.Play(_baseAnim);
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
