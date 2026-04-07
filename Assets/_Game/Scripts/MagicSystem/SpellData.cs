using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Magic System/New Spell Data")]
public class SpellData : ScriptableObject
{
    [Header("Attack damage params")]
    public int PhysicalDamage = 10;
    public int StellarDamage = 0;
    public int FireDamage = 0;
    public int LightiningDamage = 0;


    [Header("Status buildup params")]
    public int PoisonBuildup = 0;
    public int FreezeBuildup = 0;
    public int IchorBuildup = 0;

    [Header("Movement params")]
    public float ProjectileSpeed = 1f;
    public float MaxDuration = 1f;
    public float GravityModifier = 0f;
    public float CastAngle = 0f;
}
