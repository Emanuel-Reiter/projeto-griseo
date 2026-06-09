using UnityEngine;

public class PlayerStateAtkSpecial1 : PlayerBaseState
{
    [Header("Transitions")]
    [SerializeField] private PlayerBaseState _idleState;

    private bool _queueAttack = false;
    private int _atkPushTimer;

    [Header("Atk grunt Sfx")]
    [SerializeField] private AudioClip[] _atkGruntSfx;

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
            manager.Deps.Locomotion.PushByDirectionComplex(-manager.transform.right, 1f);
        });

        if (_atkGruntSfx != null)
        {
            int audioIndex = Random.Range(0, _atkGruntSfx.Length);
            Debug.Log(audioIndex);
            AudioPool.Play(_atkGruntSfx[audioIndex], default, false, default, 0.4f);
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
