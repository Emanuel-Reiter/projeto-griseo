using UnityEngine;

public class PlayerStateAtkBasic : PlayerBaseState
{
    [Header("Transitions")]
    [SerializeField] private PlayerBaseState _idleState;

    private bool _queueAttack = false;
    private int _atkPushTimer;

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
            manager.Deps.Locomotion.PushByDirectionComplex(-manager.transform.right, 0.5f);
        });
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
        bool atkInput = manager.Deps.Input.IsAttackBasicPressed;
        if (GetCurrentTime() > GetStateCompletion(0.667f) && atkInput) _queueAttack = true;

        if (GetCurrentTime() < GetStateCompletion(0.2f))
        {
            manager.Deps.Locomotion.ChangeDirectionByInput(manager.Deps.Input.MoveDirInput.x);
        }
    }
}
