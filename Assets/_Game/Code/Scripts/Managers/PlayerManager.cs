using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private GameObject _playerRef;
    public GameObject PlayerRef => _playerRef;

    private void Start()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");
    }
}
