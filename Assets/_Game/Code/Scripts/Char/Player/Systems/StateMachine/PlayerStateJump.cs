using UnityEngine;

public class PlayerStateJump : PlayerBaseState
{
    [SerializeField] private PlayerBaseState _fallState;

    [Header("Jump grunt Sfx")]
    [SerializeField] private AudioClip[] _jumpGruntSfx;
    private bool _canPlayJumpSfx = true;

    public override void CheckExitState(PlayerStateManager manager)
    {
        if (manager.Deps.Locomotion.GetVelocity().y <= 0f)
        {
            manager.SwitchState(_fallState);
            return;
        }

        if (manager.Deps.JumpManager.CanJump() && manager.Deps.Input.Jump.Pressed)
        {
            manager.SwitchState(this);
            return;
        }
    }

    public override void EnterState(PlayerStateManager manager)
    {
        manager.Deps.JumpManager.ConsumeJumps();
        manager.Deps.Locomotion.PushByDirectionComplex(Vector2.up, manager.Deps.MoveData.JumpHeight);

        if (_jumpGruntSfx != null && _canPlayJumpSfx)
        {
            int audioIndex = Random.Range(0, _jumpGruntSfx.Length);
            Debug.Log(audioIndex);
            AudioPool.Play(_jumpGruntSfx[audioIndex], default, false, default, 0.2f);
        }
        //_canPlayJumpSfx = !_canPlayJumpSfx;
    }

    public override void ExitState(PlayerStateManager manager)
    {
    }

    public override void PhysicsUpdateState(PlayerStateManager manager)
    {
        Vector2 inputDir = manager.Deps.Input.MoveDir;
        manager.Deps.Locomotion.Move(manager.Deps.MoveData.RunSpeed, manager.Deps.MoveData.BaseAcceleration, inputDir.x);
    }

    public override void UpdateState(PlayerStateManager manager)
    {
        manager.Deps.Locomotion.ChangeDirectionByInput(manager.Deps.Input.MoveDir.x);

        if (manager.Deps.Input.Jump.Released)
        {
            float newYvel = manager.Deps.Locomotion.GetVelocity().y * 0.5f;
            manager.Deps.Locomotion.SetYVelocity(newYvel);
        }
    }
}
