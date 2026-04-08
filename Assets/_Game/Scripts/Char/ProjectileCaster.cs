using UnityEngine;

public class ProjectileCaster : MonoBehaviour
{
    public void Cast(CastData cast, Vector3 castPoint, bool isFacingRight)
    {
        Quaternion castRot = isFacingRight ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);

        for (int i = cast.AmountOfCasts; i > 0; i--)
        {
            GameObject castedProjectile = Instantiate(cast.Projectile, castPoint, castRot, null);
            {
                if (castedProjectile.TryGetComponent<Projectile>(out Projectile projectile)) projectile.FireProjectile(cast, isFacingRight);
            }
        }
    }
}
