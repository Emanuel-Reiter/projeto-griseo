using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerDependencies _deps;

    private void Awake()
    {
        _deps = GetComponentInParent<PlayerDependencies>();
    }

    public void TriggerCast()
    {
        _deps.Equipment.CastBasicSpell();
    }
}
