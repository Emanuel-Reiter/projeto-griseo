using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerManager : Singleton<PlayerManager>
{
    private GameObject _playerRef;
    public GameObject PlayerRef => _playerRef;

    private PlayerDependencies _deps;
    public PlayerDependencies Deps => _deps;

    private Light2D _playerLantern;
    public Light2D PlayerLantern => _playerLantern;

    private void Start()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        _playerLantern = GameObject.FindGameObjectWithTag("PlayerLantern").GetComponent<Light2D>();

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

            UiHudManager.I.FadeToggle(true);
        }
        else
        {
            _deps.Locomotion.ToggleMovement(false);
            _deps.Input.EnableOnMenuControls();

            UiHudManager.I.FadeToggle(false);
        }
    }

    public void LoadPlayerOnLevel(Transform spawnPoint)
    {
        TogglePlayer(false);

        _playerRef.transform.position = spawnPoint.position;

        _deps.Attributes.ReloadAttributes();
    }

    public void SetPlayerLantern(float intensity)
    {
        if (_playerLantern == null) return;

        _playerLantern.intensity = intensity;
    }

    private void Update()
    {
        if (_playerRef == null) return;
    }
}
