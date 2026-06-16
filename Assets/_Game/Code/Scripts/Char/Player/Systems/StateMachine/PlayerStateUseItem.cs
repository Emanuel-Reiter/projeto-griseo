using UnityEngine;

public class PlayerStateUseItem : PlayerBaseState
{
    [SerializeField] private PlayerBaseState _idleState;

    [Header("Vfx")]
    [SerializeField] private ParticleSystem _healVfx;

    public override void CheckExitState(PlayerStateManager manager)
    {
        if (HasCompletedExitTime())
        {
            manager.SwitchState(_idleState);
            return;
        }
    }

    public override void EnterState(PlayerStateManager manager)
    {
        manager.Deps.Inventory.UseHealthPotion();

        if (_healVfx != null) _healVfx.Play();
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
