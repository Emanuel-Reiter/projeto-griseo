using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private GameObject _playerRef;
    public GameObject PlayerRef => _playerRef;

    private PlayerDependencies _deps;

    private void Start()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");

        if (_playerRef != null)
        {
            _deps = _playerRef.GetComponent<PlayerDependencies>();
        }

        TogglePlayer(false);
    }

    public void TogglePlayer(bool toggle)
    {
        if (toggle)
        {
            _deps.Locomotion.ToggleMovement(true);
            _deps.Input.EnableInGameControls();
        }
        else
        {
            _deps.Locomotion.ToggleMovement(false);
            _deps.Input.EnableOnMenuControls();
        }
    }

    public void LoadPlayerOnLevel(Transform spawnPoint)
    {
        TogglePlayer(false);

        _playerRef.transform.position = spawnPoint.position;

        _deps.Attributes.ReloadAttributes();
    }
}
