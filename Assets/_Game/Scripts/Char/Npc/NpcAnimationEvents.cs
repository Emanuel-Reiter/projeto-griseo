using UnityEngine;

public class NpcAnimationEvents : MonoBehaviour
{
    private NpcDependencies _deps;

    private void Awake()
    {
        _deps = GetComponentInParent<NpcDependencies>();
    }

    public void TriggerCast()
    {
        _deps.Equipment.CastBasicSpell();
    }
}
