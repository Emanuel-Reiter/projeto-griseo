using UnityEngine;

public class PlayerStateAtkBasic2 : PlayerBaseState
{
    [Header("Transitions")]
    [SerializeField] private PlayerBaseState _idleState;

    private bool _queueAttack = false;

    [Header("Atk grunt Sfx")]
    [SerializeField] private AudioClip[] _atkGruntSfx;
    private int _currentSfxIndex = 0;
    private bool _canPlayAtkSfx = true;

    private bool _lastAtkSfxPlayed = false;
    private float _repeatPenalty = 0.25f;

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

        float playProbability = _lastAtkSfxPlayed ? _repeatPenalty : (1f - _repeatPenalty);
        _canPlayAtkSfx = Random.value < playProbability;
        _lastAtkSfxPlayed = _canPlayAtkSfx;

        if (_atkGruntSfx != null && _canPlayAtkSfx)
        {
            _currentSfxIndex = (_currentSfxIndex + 1) % _atkGruntSfx.Length;
            AudioPool.Play(_atkGruntSfx[_currentSfxIndex], default, false, default, 0.4f, Random.Range(0.95f, 1.05f));
        }

        //_canPlayAtkSfx = !_canPlayAtkSfx;
    }

    public override void ExitState(PlayerStateManager manager)
    {

    }

    public override void PhysicsUpdateState(PlayerStateManager manager)
    {
        manager.Deps.Locomotion.Decelerate(manager.Deps.MoveData.BaseAcceleration);
    }

    public override void UpdateState(PlayerStateManager manager)
    {
        bool atkInput = manager.Deps.Input.AttackBasic2.Pressed;
        if (GetCurrentTime() > GetStateCompletion(0.667f) && atkInput) _queueAttack = true;

        if (GetCurrentTime() < GetStateCompletion(0.2f))
        {
            manager.Deps.Locomotion.ChangeDirectionByInput(manager.Deps.Input.MoveDir.x);
        }
    }
}
