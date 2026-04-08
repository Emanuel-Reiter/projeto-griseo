using UnityEngine;

public class ProjectileCaster : MonoBehaviour
{
    public void Cast(CastData cast, Vector3 castPoint, bool isFacingRight)
    {
        Quaternion castRot = isFacingRight ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);

        for (int i = cast.AmountOfCasts; i > 0; i--)
        {
            GameObject castedProjectile = Instantiate(cast.Projectile, castPoint, castRot, null);

            Vector2 initialForceLocal = cast.InitialForce * cast.InitialDirection.normalized;
            Vector2 constantVelocityLocal = cast.ConstantVelocity * cast.ConstantDirection.normalized;

            if (cast.CastScatteringAngle > 0f)
            {
                float halfAngle = cast.CastScatteringAngle * 0.5f;
                float randomAngleDeg = Random.Range(-halfAngle, halfAngle);
                float randomAngleRad = randomAngleDeg * Mathf.Deg2Rad;

                initialForceLocal = RotateVector(initialForceLocal, randomAngleRad);
                constantVelocityLocal = RotateVector(constantVelocityLocal, randomAngleRad);
            }

            if (castedProjectile.TryGetComponent<Projectile>(out Projectile projectile))
            {
                projectile.FireProjectile(cast.ProjectileDuration, cast.GravityModifier, initialForceLocal, constantVelocityLocal);
            }
        }
    }

    private Vector2 RotateVector(Vector2 v, float radians)
    {
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}
