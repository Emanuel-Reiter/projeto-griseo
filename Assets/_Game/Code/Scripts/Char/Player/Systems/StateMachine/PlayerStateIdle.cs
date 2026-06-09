using UnityEngine;

public class PlayerStateIdle : PlayerBaseState
{
    [Header("Transitions")]
    [SerializeField] private PlayerBaseState _moveState;
    [SerializeField] private PlayerBaseState _fallState;
    [SerializeField] private PlayerBaseState _jumpState;

    [Header("Attack transitions")]
    [SerializeField] private PlayerBaseState _atkBasic1State;
    [SerializeField] private PlayerBaseState _atkBasic2State;
    [SerializeField] private PlayerBaseState _atkSpecial1State;
    [SerializeField] private PlayerBaseState _atkSpecial2State;

    public override void CheckExitState(PlayerStateManager manager)
    {
        // Fall
        if(!manager.Deps.EnvDetection.IsGrounded)
        {
            manager.SwitchState(_fallState);
            return;
        }

        // Jump
        if (manager.Deps.JumpManager.CanJump() && manager.Deps.Input.Jump.Pressed)
        {
            manager.SwitchState(_jumpState);
            return;
        }

        // Locomotion
        if (manager.Deps.Input.MoveDir.x != 0f)
        {
            manager.SwitchState(_moveState);
            return;
        }

        // Attack Basic 1
        if (manager.Deps.Input.AttackBasic1.Pressed)
        {
            if (manager.Deps.EnvDetection.IsGrounded)
            {
                manager.SwitchState(_atkBasic1State);
                return;
            }
        }

        // Attack Basic 2
        if (manager.Deps.Input.AttackBasic2.Pressed)
        {
            if (manager.Deps.EnvDetection.IsGrounded)
            {
                manager.SwitchState(_atkBasic2State);
                return;
            }
        }

        // Attack Special 1
        if (manager.Deps.Input.AttackSpecial1.Pressed)
        {
            if (manager.Deps.EnvDetection.IsGrounded)
            {
                manager.SwitchState(_atkSpecial1State);
                return;
            }
        }

        // Attack Special 2
        if (manager.Deps.Input.AttackSpecial2.Pressed)
        {
            if (manager.Deps.EnvDetection.IsGrounded)
            {
                manager.SwitchState(_atkSpecial2State);
                return;
            }
        }
    }

    public override void EnterState(PlayerStateManager manager)
    {
        manager.Deps.JumpManager.RestoreJumps();
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

    }
}
