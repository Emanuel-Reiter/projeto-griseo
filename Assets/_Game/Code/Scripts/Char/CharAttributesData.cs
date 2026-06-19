using UnityEngine;

[CreateAssetMenu(fileName = "AttributesData", menuName = "Character/AttributesData")]
public class CharAttributesData : ScriptableObject
{
    [Min(0)] public int MaxHealth = 1;
    [Min(0)] public int MaxMana = 1;
    [Range(0f, 1f)] public float KbResistence = 0f;

    private void OnValidate()
    {
        if (MaxHealth < 1)
        {
            MaxHealth = 1;
        }

        if (MaxMana <= 0)
        {
            MaxMana = 0;
        }
    }
}
