public struct DamageData
{
    // Damage
    public int BaseDMGPhysical;
    public int BaseDMGStellar;
    public int BaseDMGFire;
    public int BaseDMGLightining;

    // Status
    public int STSPoison;
    public int STSFrostbite;
    public int STSIchor;

    // Crit
    public float CritChance;
    public float CritModifier;

    // Knockback
    public float KnockbackForce;
    public bool IsFacingRight;

    public DamageData(int dmgPhysical, int dmgStellar, int dmgFire, int dmgLightining, int stsPoison, int stsFrostbite, int stsIchor, float critChance, float critModifier, float knockback, bool isFacingRight)
    {
        BaseDMGPhysical = dmgPhysical;
        BaseDMGStellar = dmgStellar;
        BaseDMGFire = dmgFire;
        BaseDMGLightining = dmgLightining;

        STSPoison = stsPoison;
        STSFrostbite = stsFrostbite;
        STSIchor = stsIchor;

        CritChance = critChance;
        CritModifier = critModifier;

        KnockbackForce = knockback;
        IsFacingRight = isFacingRight;
    }
}
