using UnityEngine;
using static UnityEditor.Progress;

public class PlayerStateJump : PlayerBaseState
{
    [SerializeField] private PlayerBaseState _fallState;

    [Header("Jump grunt Sfx")]
    [SerializeField] private AudioClip[] _jumpGruntSfx;
    private bool _canPlayJumpSfx = true;
    private int _currentSfxIndex = 0;

    private bool _lastJumpSfxPlayed = false;
    private float _repeatPenalty = 0.25f;

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

        float playProbability = _lastJumpSfxPlayed ? _repeatPenalty : (1f - _repeatPenalty);
        _canPlayJumpSfx = Random.value < playProbability;
        _lastJumpSfxPlayed = _canPlayJumpSfx;

        if (_jumpGruntSfx != null && _canPlayJumpSfx)
        {
            _currentSfxIndex = (_currentSfxIndex + 1) % _jumpGruntSfx.Length;
            AudioPool.Play(_jumpGruntSfx[_currentSfxIndex], default, false, default, 0.3f, Random.Range(0.95f, 1.1f));
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
