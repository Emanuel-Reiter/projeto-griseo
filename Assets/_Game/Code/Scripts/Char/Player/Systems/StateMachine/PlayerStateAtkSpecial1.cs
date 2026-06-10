using UnityEngine;

public class PlayerStateAtkSpecial1 : PlayerBaseState
{
    [Header("Transitions")]
    [SerializeField] private PlayerBaseState _idleState;

    private bool _queueAttack = false;
    private int _atkPushTimer;

    [Header("Atk grunt Sfx")]
    [SerializeField] private AudioClip[] _atkGruntSfx;
    private int _currentSfxIndex = 0;

    public override void CheckExitState(PlayerStateManager manager)
    {
        if (!HasCompletedExitTime()) return;

        // Attack queue
        if (_queueAttack)
        {
            manager.SwitchState(this);
            return;
        }

        manager.SwitchState(_idleState);
        return;
    }

    public override void EnterState(PlayerStateManager manager)
    {
        if (_baseAnim == null) return;

        SetStateDuration(_baseAnim.length);
        _queueAttack = false;
        manager.Deps.CharAnimator.Play(_baseAnim);

        _atkPushTimer = TimerManager.I.StartTimer(GetStateCompletion(0.75f), () =>
        {
            if (this != null) manager.Deps.Locomotion.PushByDirectionComplex(-manager.transform.right, 1f);
        });

        if (_atkGruntSfx != null)
        {
            _currentSfxIndex = (_currentSfxIndex + 1) % _atkGruntSfx.Length;
            AudioPool.Play(_atkGruntSfx[_currentSfxIndex], default, false, default, 0.4f, Random.Range(0.95f, 1.05f));
        }
    }

    public override void ExitState(PlayerStateManager manager)
    {
        TimerManager.I.CancelTimer(_atkPushTimer);
    }

    public override void PhysicsUpdateState(PlayerStateManager manager)
    {
        manager.Deps.Locomotion.Decelerate(manager.Deps.MoveData.BaseAcceleration);
    }

    public override void UpdateState(PlayerStateManager manager)
    {
        bool atkInput = manager.Deps.Input.AttackSpecial1.Pressed;
        if (GetCurrentTime() > GetStateCompletion(0.667f) && atkInput) _queueAttack = true;

        if (GetCurrentTime() < GetStateCompletion(0.2f))
        {
            manager.Deps.Locomotion.ChangeDirectionByInput(manager.Deps.Input.MoveDir.x);
        }
    }
}
