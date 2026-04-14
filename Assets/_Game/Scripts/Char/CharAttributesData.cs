using UnityEngine;

[CreateAssetMenu(fileName = "AttributesData", menuName = "Character/AttributesData")]
public class CharAttributesData : ScriptableObject
{
    [Min(0)] public int MaxHealth = 1;

    private void OnValidate()
    {
        if (MaxHealth < 1)
        {
            MaxHealth = 1;
        }
    }
}
