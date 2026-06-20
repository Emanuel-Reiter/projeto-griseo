using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipment : MonoBehaviour
{
    private PlayerDependencies _deps;

    [SerializeField] private SpellData _equipedBasicSpell1;
    [SerializeField]  private SpellData _equipedBasicSpell2;

    [SerializeField] private SpellData _equipedSpecialSpell1;
    public SpellData EquipedSpecialSpell1 => _equipedSpecialSpell1;

    [SerializeField] private SpellData _equipedSpecialSpell2;
    public SpellData EquipedSpecialSpell2 => _equipedSpecialSpell2;

    [Header("Ui hotbar spells")]
    [SerializeField] private Image _equipedSpecialSpell1Image;
    [SerializeField] private Image _equipedSpecialSpell2Image;

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();

        _deps.Attributes.OnManaChangeEvent += UpdateSpellHotbar;
    }

    private void OnDestroy()
    {
        _deps.Attributes.OnManaChangeEvent -= UpdateSpellHotbar;
    }

    private void UpdateSpellHotbar(int newMana)
    {
        Color defaultColor = new Color(1f, 1f, 1f, 1f);
        Color disabledColor = new Color(1f, 1f, 1f, 0.05f);

        if (_equipedSpecialSpell1Image != null && _equipedSpecialSpell1 != null)
        {
            if (newMana < _equipedSpecialSpell1.ManaCost) _equipedSpecialSpell1Image.color = disabledColor;
            else _equipedSpecialSpell1Image.color = defaultColor;
        }

        if (_equipedSpecialSpell2Image != null && _equipedSpecialSpell2 != null)
        {
            if (newMana < _equipedSpecialSpell2.ManaCost) _equipedSpecialSpell2Image.color = disabledColor;
            else _equipedSpecialSpell2Image.color = defaultColor;
        }
    }

    public void CastBasicSpell1()
    {
        if (_equipedBasicSpell1 == null) return;
        if (_deps.Attributes.CurrentMana < _equipedBasicSpell1.ManaCost) return;


        _deps.Cast.Cast(_equipedBasicSpell1, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight, _deps.Attributes);
        ConsumeMana(_equipedBasicSpell1);
    }

    public void CastBasicSpell2()
    {
        if (_equipedBasicSpell2 == null) return;
        if (_deps.Attributes.CurrentMana < _equipedBasicSpell2.ManaCost) return;

        _deps.Cast.Cast(_equipedBasicSpell2, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight, _deps.Attributes);
        ConsumeMana(_equipedBasicSpell2);
    }

    public void CastSpecialSpell1()
    {
        if (_equipedSpecialSpell1 == null) return;
        if (_deps.Attributes.CurrentMana < _equipedSpecialSpell1.ManaCost) return;


        _deps.Cast.Cast(_equipedSpecialSpell1, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight, _deps.Attributes);
        ConsumeMana(_equipedSpecialSpell1);
    }

    public void CastSpecialSpell2()
    {
        if (_equipedSpecialSpell2 == null) return;
        if (_deps.Attributes.CurrentMana < _equipedSpecialSpell2.ManaCost) return;


        _deps.Cast.Cast(_equipedSpecialSpell2, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight, _deps.Attributes);
        ConsumeMana(_equipedSpecialSpell2);
    }

    private void ConsumeMana(SpellData spell)
    {
        _deps.Attributes.CurrentMana -= spell.ManaCost;
    }
}
