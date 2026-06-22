using UnityEngine;

public class NpcChaserIdleState : NpcBaseState
{
    [Header("State transitions")]
    [SerializeField] private NpcBaseState _atkState;
    [SerializeField] private NpcBaseState _chaseState;

    [Header("Other params")]
    [SerializeField] private float _minAttackDsistance = 10f;

    public override void CheckExitState(NpcStateManager manager)
    {
        if (!HasCompletedExitTime()) return;

        if (manager.Deps.TargetDetection.HasTarget)
        {
            if (manager.Deps.TargetDetection.GetDistanceFromTarget() < _minAttackDsistance)
            {
                manager.SwitchState(_atkState);
                return;
            }
            else
            {
                manager.SwitchState(_chaseState);
                return;
            }
        }
    }

    public override void EnterState(NpcStateManager manager)
    {
        if (_baseAnim == null) return;

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
