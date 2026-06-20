using UnityEngine;

public class SpellProjectileCaster : MonoBehaviour
{
    public void Cast(SpellData spell, Vector3 castPoint, bool isFacingRight, CharAttributesManager casterAttributes)
    {
        Quaternion castRot = isFacingRight ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);

        for (int i = spell.AmountOfCasts; i > 0; i--)
        {
            GameObject castedProjectile = Instantiate(spell.Projectile, castPoint, castRot, null);
            {
                if (castedProjectile.TryGetComponent<SpellProjectile>(out SpellProjectile projectile)) projectile.FireProjectile(spell, isFacingRight, casterAttributes);
            }
        }
    }
}
