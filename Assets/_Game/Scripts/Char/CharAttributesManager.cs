using UnityEngine;

public class CharAttributesManager : MonoBehaviour
{
    private CharLocomotion _locomotion;

    private void Awake()
    {
        _locomotion = GetComponent<CharLocomotion>();
    }

    public void TakeDamage(DamageData damageData)
    {
        // Apply knockback
        Debug.Log($"{gameObject.name} damaged, kb: {damageData.KnockbackForce}");
        Vector2 kbDir = new Vector2(damageData.KnockbackDir.x, 0.2f).normalized;
        _locomotion.PushByDirectionRaw(kbDir, damageData.KnockbackForce);
    }
}
