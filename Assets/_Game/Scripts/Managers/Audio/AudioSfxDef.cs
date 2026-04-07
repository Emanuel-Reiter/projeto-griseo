using UnityEngine;

[CreateAssetMenu(menuName = "Audio/SFX Def", fileName = "SfxDef")]
public class AudioSfxDef : ScriptableObject
{
    public AudioClip Clip;

    [Range(0, 256)] public int Priority = 128;
    public bool is3D = true;

    [Range(0f, 1f)] public float Volume = 1f;
    [Range(-3f, 3f)] public float Pitch = 1f;

    public float MinDistance = 1f;
    public float MaxDistance = 25f;

    public bool Loop = false;
}
