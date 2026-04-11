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
        Vector2 dir = damageData.IsFacingRight ? Vector2.right : Vector2.left;
        _locomotion.PushByDirectionSimple(dir, damageData.KnockbackForce);
    }
}
