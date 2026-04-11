using UnityEngine;

public class PlayerStateJump : PlayerBaseState
{
    [SerializeField] private PlayerBaseState _fallState;

    public override void CheckExitState(PlayerStateManager manager)
    {
        if (manager.Deps.Locomotion.GetVelocity().y <= 0f)
        {
            manager.SwitchState(_fallState);
            return;
        }
    }

    public override void EnterState(PlayerStateManager manager)
    {
        manager.Deps.Locomotion.PushByDirectionComplex(Vector2.up, manager.Deps.MoveData.JumpHeight);
    }

    public override void ExitState(PlayerStateManager manager)
    {

    }

    public override void PhysicsUpdateState(PlayerStateManager manager)
    {
        Vector2 inputDir = manager.Deps.Input.MoveDirInput;
        manager.Deps.Locomotion.Move(manager.Deps.MoveData.RunSpeed, manager.Deps.MoveData.BaseAcceleration, inputDir);
    }

    public override void UpdateState(PlayerStateManager manager)
    {
        manager.Deps.Locomotion.ChangeDirectionByInput(manager.Deps.Input.MoveDirInput);
    }
}
