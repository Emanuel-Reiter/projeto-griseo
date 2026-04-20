using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStateIdle : PlayerBaseState
{
    [Header("Transitions")]
    [SerializeField] private PlayerBaseState _moveState;
    [SerializeField] private PlayerBaseState _fallState;
    [SerializeField] private PlayerBaseState _jumpState;

    [Header("Attack transitions")]
    [SerializeField] private PlayerBaseState _atkLightState;

    public override void CheckExitState(PlayerStateManager manager)
    {
        // Fall
        if(!manager.Deps.EnvDetection.IsGrounded)
        {
            manager.SwitchState(_fallState);
            return;
        }

        // Jump
        if(manager.Deps.EnvDetection.IsGrounded && manager.Deps.Input.IsJumpPressed)
        {
            manager.SwitchState(_jumpState);
            return;
        }

        // Locomotion
        if (manager.Deps.Input.MoveDirInput.x != 0f)
        {
            manager.SwitchState(_moveState);
            return;
        }

        // Attack
        if (manager.Deps.Input.IsAttackBasicPressed)
        {
            if (manager.Deps.EnvDetection.IsGrounded)
            {
                manager.SwitchState(_atkLightState);
                return;
            }
        }
    }

    public override void EnterState(PlayerStateManager manager)
    {
        manager.Deps.DoubleJump.RestoreJumps();
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
