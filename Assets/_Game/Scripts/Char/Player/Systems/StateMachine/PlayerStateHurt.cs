using UnityEngine;

public class PlayerStateHurt : PlayerBaseState
{
    [Header("Transitions")]
    [SerializeField] private PlayerBaseState _idle;

    public override void CheckExitState(PlayerStateManager manager)
    {
        // Idle
        if(HasCompletedExitTime())
        {
            manager.SwitchState(_idle);
            return;
        }
    }

    public override void EnterState(PlayerStateManager manager)
    {

    }

    public override void ExitState(PlayerStateManager manager)
    {

    }

    public override void PhysicsUpdateState(PlayerStateManager manager)
    {
        manager.Deps.Locomotion.Decelerate(manager.Deps.MovementData.BaseAcceleration);
    }

    public override void UpdateState(PlayerStateManager manager)
    {

    }
}
