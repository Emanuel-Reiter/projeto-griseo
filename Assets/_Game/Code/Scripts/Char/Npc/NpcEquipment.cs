using UnityEngine;

public class NpcEquipment : MonoBehaviour
{
    private NpcDependencies _deps;

    [SerializeField] private CastData _equipedSpell;

    private void Start()
    {
        _deps = GetComponent<NpcDependencies>();
    }

    public void CastBasicSpell()
    {
        _deps.Cast.Cast(_equipedSpell, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight);
    }
}
