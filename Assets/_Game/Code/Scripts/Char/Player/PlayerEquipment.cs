using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private PlayerDependencies _deps;

    [SerializeField] private SpellData _equipedBasicSpell1;
    [SerializeField]  private SpellData _equipedBasicSpell2;

    [SerializeField] private SpellData _equipedSpecialSpell1;
    [SerializeField] private SpellData _equipedSpecialSpell2;

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();
    }

    public void CastBasicSpell1()
    {
        if (_equipedBasicSpell1 == null) return;
        _deps.Cast.Cast(_equipedBasicSpell1, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight);
    }

    public void CastBasicSpell2()
    {
        if (_equipedBasicSpell2 == null) return;
        _deps.Cast.Cast(_equipedBasicSpell2, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight);
    }

    public void CastSpecialSpell1()
    {
        if (_equipedSpecialSpell1 == null) return;
        _deps.Cast.Cast(_equipedSpecialSpell1, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight);
    }

    public void CastSpecialSpell2()
    {
        if (_equipedSpecialSpell2 == null) return;
        _deps.Cast.Cast(_equipedSpecialSpell2, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight);
    }

    #region Debug UI
    private int GUIPositionY(int row, int height)
    {
        return row * height;
    }

    private void OnGUI()
    {
        int xOffset = 48;
        int width = 512;
        int height = 48;

        GUI.skin.label.fontSize = 32;

        GUI.Label(new Rect(xOffset, GUIPositionY(3, height), width, height), $"Spell: {_equipedBasicSpell1.SpellName}");
    }
    #endregion
}
