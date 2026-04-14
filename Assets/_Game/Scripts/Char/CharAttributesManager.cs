using System;
using UnityEngine;

public class CharAttributesManager : MonoBehaviour
{
    private CharLocomotion _locomotion;

    [SerializeField] private CharAttributesData _charAttributes;
    public CharAttributesData CharAttributes => _charAttributes;

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

            _currentHealth = value;
            OnHealthChangeEvent?.Invoke(CurrentHealth);
        }
    }

    public delegate void OnHealthChangeDelegate(int health);
    public event OnHealthChangeDelegate OnHealthChangeEvent;

    public delegate void OnTakeDamageDelegate();
    public event OnTakeDamageDelegate OnTakeDamageEvent;

    public delegate void OnDieDelegate();
    public event OnDieDelegate OnDieEvent;

    private void Awake()
    {
        _locomotion = GetComponent<CharLocomotion>();

        if (_charAttributes == null) Debug.LogError($"Char: {gameObject.name} doesn't have a attributes data assigned");

        CurrentHealth = _charAttributes.MaxHealth;
    }

    public void TakeDamage(DamageData damageData)
    {
        // knockback
        Vector2 kbDir = new Vector2(damageData.KnockbackDir.x, 0.2f).normalized;
        _locomotion.PushByDirectionRaw(kbDir, damageData.KnockbackForce);

        // Damage
        //Debug.Log($"Char: {gameObject.name} took damage: {CalculateFinalDamage(damageData)}");
        CurrentHealth -= CalculateFinalDamage(damageData);
    }

    private int CalculateFinalDamage(DamageData damageData)
    {
        int finalDamage = 0;

        finalDamage += damageData.BaseDMGPhysical;
        finalDamage += damageData.BaseDMGStellar;
        finalDamage += damageData.BaseDMGFire;
        finalDamage += damageData.BaseDMGLightining;

        OnTakeDamageEvent?.Invoke();

        return finalDamage;
    }

    private void Die()
    {
        OnDieEvent?.Invoke();
        gameObject.SetActive(false);
    }
}
