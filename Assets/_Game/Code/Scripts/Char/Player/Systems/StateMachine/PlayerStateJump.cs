using UnityEngine;

public class PlayerStateJump : PlayerBaseState
{
    [SerializeField] private PlayerBaseState _fallState;
    bool _allowDoubleJump = false;

    public override void CheckExitState(PlayerStateManager manager)
    {
        if (manager.Deps.Locomotion.GetVelocity().y <= 0f)
        {
            manager.SwitchState(_fallState);
            return;
        }

        if (_allowDoubleJump && manager.Deps.Input.IsJumpPressed && manager.Deps.DoubleJump.JumpsRemaining > 0)
        {
            manager.Deps.DoubleJump.ConsumeJumps();
            manager.SwitchState(this);
            return;
        }
    }

    public override void EnterState(PlayerStateManager manager)
    {
        _allowDoubleJump = false;
        manager.Deps.Locomotion.PushByDirectionComplex(Vector2.up, manager.Deps.MoveData.JumpHeight);
        TimerManager.I.StartTimer(0.15f, () => { _allowDoubleJump = true; });
    }

    public override void ExitState(PlayerStateManager manager)
    {
        _allowDoubleJump = false;
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
