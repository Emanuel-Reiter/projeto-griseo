using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private PlayerDependencies _deps;

    [SerializeField] private CastData _equipedSpell;

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();
    }

    public void CastBasicSpell()
    {
        _deps.Cast.Cast(_equipedSpell, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight);
    }
}
