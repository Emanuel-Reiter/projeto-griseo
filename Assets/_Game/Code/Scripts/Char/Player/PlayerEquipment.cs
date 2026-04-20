using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private PlayerDependencies _deps;

    private SpellData _equipedSpell;

    [SerializeField] private SpellData[] _spells;
    private int _equipedSpellIndex = 0;

    [Header("Audio")]
    [SerializeField] private AudioClip _spellSwitchHintSfx;

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();
        _equipedSpell = _spells[_equipedSpellIndex];
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            _equipedSpellIndex++;
            ValidateSpellIndex();
            _equipedSpell = _spells[_equipedSpellIndex];
            AudioPool.Play(_spellSwitchHintSfx, default, false);
        }
        else if (scroll < 0f)
        {
            _equipedSpellIndex--;
            ValidateSpellIndex();
            _equipedSpell = _spells[_equipedSpellIndex];
            AudioPool.Play(_spellSwitchHintSfx, default, false);
        }
    }

    private void ValidateSpellIndex()
    {
        if (_equipedSpellIndex > _spells.Length - 1) _equipedSpellIndex = 0;
        if (_equipedSpellIndex < 0) _equipedSpellIndex = _spells.Length - 1;
    }

    public void CastBasicSpell()
    {
        if (_equipedSpell == null) return;
        _deps.Cast.Cast(_equipedSpell, _deps.SpellOrigin.position, _deps.Locomotion.IsFacingRight);
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

        GUI.Label(new Rect(xOffset, GUIPositionY(3, height), width, height), $"Spell: {_equipedSpell.SpellName}");
    }
    #endregion
}
