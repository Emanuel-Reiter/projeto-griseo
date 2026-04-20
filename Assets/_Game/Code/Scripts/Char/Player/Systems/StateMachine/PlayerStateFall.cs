using UnityEngine;

public class PlayerStateFall : PlayerBaseState
{
    [SerializeField] private PlayerBaseState _idleState;
    [SerializeField] private PlayerBaseState _jumpState;

    public override void CheckExitState(PlayerStateManager manager)
    {
        if (manager.Deps.EnvDetection.IsGrounded)
        {
            manager.SwitchState(_idleState);
            return;
        }

        if (manager.Deps.JumpManager.CanJump() && manager.Deps.Input.IsJumpPressed)
        {
            manager.SwitchState(_jumpState);
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
        Vector2 inputDir = manager.Deps.Input.MoveDirInput;
        manager.Deps.Locomotion.Move(manager.Deps.MoveData.RunSpeed, manager.Deps.MoveData.BaseAcceleration, inputDir.x);
    }

    public override void UpdateState(PlayerStateManager manager)
    {
        manager.Deps.Locomotion.ChangeDirectionByInput(manager.Deps.Input.MoveDirInput.x);
    }
}
