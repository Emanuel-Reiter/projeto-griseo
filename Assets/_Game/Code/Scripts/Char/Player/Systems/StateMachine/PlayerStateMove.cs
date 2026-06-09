using UnityEngine;

public class PlayerStateMove : PlayerBaseState
{
    [Header("Transitions")]
    [SerializeField] private PlayerBaseState _idleState;
    [SerializeField] private PlayerBaseState _fallState;
    [SerializeField] private PlayerBaseState _jumpState;
    
    [Header("Attack transitions")]
    [SerializeField] private PlayerBaseState _atkBasic1State;
    [SerializeField] private PlayerBaseState _atkBasic2State;
    [SerializeField] private PlayerBaseState _atkSpecial1State;
    [SerializeField] private PlayerBaseState _atkSpecial2State;

    [Header("Animation float")]
    [SerializeField] private string _movementMultiplaier;

    public override void CheckExitState(PlayerStateManager manager)
    {
        // Fall
        if (!manager.Deps.EnvDetection.IsGrounded)
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

        // Idle
        if (manager.Deps.Input.MoveDir.x == 0f)
        {
            manager.SwitchState(_idleState);
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
        Vector2 inputDir = manager.Deps.Input.MoveDir;
        manager.Deps.Locomotion.Move(manager.Deps.MoveData.RunSpeed, manager.Deps.MoveData.BaseAcceleration, inputDir.x);
    }

    public override void UpdateState(PlayerStateManager manager)
    {
        manager.Deps.Locomotion.ChangeDirectionByInput(manager.Deps.Input.MoveDir.x);


        //float multiplaier = Mathf.InverseLerp(0f, manager.Deps.MoveData.RunSpeed, Mathf.Abs(manager.Deps.Locomotion.GetVelocity().x));
        //manager.Deps.CharAnimator.SetFloat(_movementMultiplaier, multiplaier);
    }

}
