using UnityEngine;

public class PlayerStateMove : PlayerBaseState
{
    [Header("Transitions")]
    [SerializeField] private PlayerBaseState _idleState;
    [SerializeField] private PlayerBaseState _fallState;
    [SerializeField] private PlayerBaseState _jumpState;
    
    [Header("Attack transitions")]
    [SerializeField] private PlayerBaseState _atkLightState;

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
        if (manager.Deps.EnvDetection.IsGrounded && manager.Deps.Input.IsJumpPressed)
        {
            manager.SwitchState(_jumpState);
            return;
        }

        // Idle
        if (manager.Deps.Input.MoveDirInput.x == 0f)
        {
            manager.SwitchState(_idleState);
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

    }

    public override void ExitState(PlayerStateManager manager)
    {

    }

    public override void PhysicsUpdateState(PlayerStateManager manager)
    {
        Vector2 inputDir = manager.Deps.Input.MoveDirInput;
        manager.Deps.Locomotion.Move(manager.Deps.MovementData.RunSpeed, manager.Deps.MovementData.BaseAcceleration, inputDir);
    }

    public override void UpdateState(PlayerStateManager manager)
    {
        manager.Deps.Locomotion.ChangeDirectionByInput(manager.Deps.Input.MoveDirInput);

        /*
        float multiplaier = Mathf.InverseLerp(0f, manager.Deps.MovementData.RunSpeed, Mathf.Abs(manager.Deps.Locomotion.GetVelocity().x));
        manager.Deps.CharAnimator.SetFloat(_movementMultiplaier, multiplaier);
        */
    }

}
