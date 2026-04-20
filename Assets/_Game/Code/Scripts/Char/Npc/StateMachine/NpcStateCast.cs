using UnityEngine;

public class NpcStateCast : NpcBaseState
{
    [Header("State transitions")]
    [SerializeField] private NpcBaseState _idleState;

    public override void CheckExitState(NpcStateManager manager)
    {
        if (!HasCompletedExitTime()) return;
        
        manager.SwitchState(_idleState);
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
        if (manager.Deps.TargetDetection.HasTarget)
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