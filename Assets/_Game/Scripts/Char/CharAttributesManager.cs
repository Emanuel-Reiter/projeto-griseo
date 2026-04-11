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
        Debug.Log($"{gameObject.name} damaged, kb: {damageData.KnockbackForce}");
        _locomotion.PushByDirectionSimple(damageData.KnockbackDir.normalized, damageData.KnockbackForce);
    }
}
