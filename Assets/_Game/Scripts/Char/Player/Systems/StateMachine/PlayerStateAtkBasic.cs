using UnityEngine;

public class PlayerStateAtkBasic : PlayerBaseState
{
    [Header("Transitions")]
    [SerializeField] private PlayerBaseState _idleState;

    [Header("Spell")]
    [SerializeField] private GameObject _basicSpell1;

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
        Vector3 eulerDir = manager.Deps.Locomotion.IsFacingRight ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 180f, 0f);
        GameObject go = Instantiate(_basicSpell1, manager.Deps.SpellOrigin.position, Quaternion.Euler(eulerDir), null);
        //Debug.Log(go.transform.eulerAngles);

        if (_baseAnim == null) return;
    
        _queueAttack = false;
        SetStateDuration(_baseAnim.length);

        _atkPushTimer = TimerManager.I.StartTimer(GetStateCompletion(0.333f), () =>
        {
            manager.Deps.Locomotion.PushByDirection(manager.transform.right, 0.5f);
        });
    }

    public override void ExitState(PlayerStateManager manager)
    {
        TimerManager.I.CancelTimer(_atkPushTimer);
    }

    public override void PhysicsUpdateState(PlayerStateManager manager)
    {
        manager.Deps.Locomotion.Decelerate(manager.Deps.MovementData.BaseAcceleration);
    }

    public override void UpdateState(PlayerStateManager manager)
    {
        bool atkInput = manager.Deps.Input.IsAttackBasicPressed;
        if (GetCurrentTime() > GetStateCompletion(0.667f) && atkInput) _queueAttack = true;

        if (GetCurrentTime() < GetStateCompletion(0.2f))
        {
            manager.Deps.Locomotion.ChangeDirectionByInput(manager.Deps.Input.MoveDirInput);
        }
    }
}
