using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level scene")]
    [SerializeField] private string _sceneName;
    public string SceneName => _sceneName;

    public bool IsValid => !string.IsNullOrWhiteSpace(_sceneName);

    [Header("Music")]
    [SerializeField] private AudioClip _levelMusic;
    public AudioClip LevelMusic => _levelMusic;
}
