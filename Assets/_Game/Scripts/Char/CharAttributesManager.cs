using UnityEngine;
using UnityEngine.SceneManagement;

public class CharAttributesManager : MonoBehaviour
{
    private CharLocomotion _locomotion;

    [SerializeField] private CharAttributesData _charAttributes;
    public CharAttributesData CharAttributes => _charAttributes;

    // Health
    private int _currentHealth = 1;
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (_currentHealth == value) return;

            if (value <= 0)
            {
                _currentHealth = 0;
                OnHealthChangeEvent?.Invoke(CurrentHealth);
                Die();
                return;
            }

            if (value > _charAttributes.MaxHealth)
            {
                _currentHealth = _charAttributes.MaxHealth;
                OnHealthChangeEvent?.Invoke(CurrentHealth);
                return;
            }

            _currentHealth = value;
            OnHealthChangeEvent?.Invoke(CurrentHealth);
        }
    }

    public delegate void OnHealthChangeDelegate(int health);
    public event OnHealthChangeDelegate OnHealthChangeEvent;

    public delegate void OnTakeDamageDelegate();
    public event OnTakeDamageDelegate OnTakeDamageEvent;

    // Mana
    private int _currentMana = 1;
    public int CurrentMana
    {
        get => _currentMana;
        set
        {
            if (_currentMana == value) return;

            if (value <= 0)
            {
                _currentMana = 0;
                OnManaChangeEvent?.Invoke(CurrentMana);
                return;
            }

            if (value > _charAttributes.MaxMana)
            {
                _currentMana = _charAttributes.MaxMana;
                OnManaChangeEvent?.Invoke(CurrentMana);
                return;
            }

            _currentMana = value;
            OnManaChangeEvent?.Invoke(CurrentMana);
        }
    }

    public delegate void OnManaChangeDelegate(int mana);
    public event OnManaChangeDelegate OnManaChangeEvent;

    // Die
    public delegate void OnDieDelegate();
    public event OnDieDelegate OnDieEvent;

    public bool HasBeenDefeated { get; private set; } = false;

    private void Start()
    {
        _locomotion = GetComponent<CharLocomotion>();

        if (_charAttributes == null) Debug.LogError($"Char: {gameObject.name} doesn't have a attributes data assigned.");

        CurrentHealth = _charAttributes.MaxHealth;
        CurrentMana = _charAttributes.MaxMana;
    }

    public void TakeDamage(DamageData damageData)
    {
        // knockback
        Vector2 kbDir = new Vector2(damageData.KnockbackDir.x, 0.2f).normalized;
        float adjustedKbForce = 1f - _charAttributes.KbResistence;
        _locomotion.PushByDirectionRaw(kbDir, damageData.KnockbackForce * adjustedKbForce);

        // Damage
        //Debug.Log($"Char: {gameObject.name} took damage: {CalculateFinalDamage(damageData)}");
        CurrentHealth -= CalculateFinalDamage(damageData);
        
        OnTakeDamageEvent?.Invoke();
    }

    private int CalculateFinalDamage(DamageData damageData)
    {
        int finalDamage = 0;

        finalDamage += damageData.BaseDMGPhysical;
        finalDamage += damageData.BaseDMGStellar;
        finalDamage += damageData.BaseDMGFire;
        finalDamage += damageData.BaseDMGLightining;

        return finalDamage;
    }

    private void Die()
    {
        // Player only
        if (gameObject.CompareTag("Player"))
        {
            _ = LevelManager.I.LoadLevel(LevelManager.I.CurrentLoadedLevel);
        }
        else
        {
            gameObject.SetActive(false);
            HasBeenDefeated = true;
        }

        OnDieEvent?.Invoke();
    }

    public void ReloadAttributes()
    {
        CurrentHealth = _charAttributes.MaxHealth;
        CurrentMana = _charAttributes.MaxMana;
    }
}
