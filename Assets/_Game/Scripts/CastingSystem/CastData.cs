using UnityEngine;

[CreateAssetMenu(fileName = "CastData", menuName = "Character/CastData")]
public class CastData : ScriptableObject
{
    [Header("Damage")]
    public int BaseDMGPhysical = 0;
    public int BaseDMGStellar = 0;
    public int BaseDMGFire = 0;
    public int BaseDMGLightining = 0;

    [Header("Status buildup")]
    public int STSPoison = 0;
    public int STSFrostbite = 0;
    public int STSIchor = 0;

    [Range(0, 99)] public int MaxTargetPenetration = 0;
    [Range(0f, 1f)] public float PerTargetDmgReduction = 0f;

    [Range(0, 99)] public int MaxEnvironmentHits = 0;
    [Range(0f, 1f)] public float PerEnvHitDmgReduction = 0f;

    [Space]
    [Header("Other damage params")]
    [Range(0f, 1f)] public float CritChace = 0f;
    [Range(1f, 99f)] public float CritModifier = 1f;

    [Space]
    [Range(0f, 999f)] public float KnockbackForce = 0f;

    [Space]
    [Range(0, 999)] public int ArmorPenetration = 0;

    [Header("Projectile params")]
    public GameObject Projectile;

    [Space]
    [Range(1, 99)] public int AmountOfCasts = 1;

    [Space]
    [Range(0f, 360f)] public float CastScatteringAngle = 0f;
    [Range(0f, 1f)] public float ProjectileTrackingPercent = 0f;
    [Range(0.1f, 300f)] public float ProjectileDuration = 0.1f;

    [Header("Initial force")]
    public float InitialForce = 0f;
    public Vector2 InitialDirection = Vector2.zero;

    [Header("Constant velocity")]
    [Range(0f, 9f)] public float ConstantVelocityStartDelay = 0f;
    public float ConstantVelocity = 0f;
    public Vector2 ConstantDirection = Vector2.zero;

    [Header("Other movement params")]
    public float DecelerationRate = 0f;
    public float GravityModifier = 0f;

    [Header("OnSpawn subcast")]
    public CastData OnSpawnCast;
    [Range(0f, 1f)] public float OnSpawnCastChance = 1f;
    [Range(0f, 60f)] public float OnSpawnCastDelay = 0f;

    [Header("OverLifetime subcast")]
    public CastData OverLifetimeCast;
    [Range(0f, 1f)] public float OverLifetimeCastChance = 1f;
    [Range(0.05f, 60f)] public float OverLifetimeCastInterval = 1f;

    [Header("OnFizzle subcast")]
    public CastData OnFizzleCast;
    [Range(0f, 1f)] public float OnFizzleCastChance = 1f;
    [Range(0f, 60f)] public float OnFizzleCastDelay = 0f;

    [Header("OnHit subcast")]
    public CastData OnHitCast;
    [Range(0f, 1f)] public float OnHitCastChance = 1f;
    [Range(0f, 60f)] public float OnHitCastDelay = 0f;
}
