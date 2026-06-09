using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerDependencies _deps;

    private void Awake()
    {
        _deps = GetComponentInParent<PlayerDependencies>();
    }

    public void TriggerBasicCast1()
    {
        _deps.Equipment.CastBasicSpell1();
    }

    public void TriggerBasicCast2()
    {
        _deps.Equipment.CastBasicSpell2();
    }

    public void TriggerSpecialCast1()
    {
        _deps.Equipment.CastSpecialSpell1();
    }

    public void TriggerSpecialCast2()
    {
        _deps.Equipment.CastSpecialSpell2();
    }
}
