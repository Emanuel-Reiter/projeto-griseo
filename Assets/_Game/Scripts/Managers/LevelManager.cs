using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Level params")]
    [SerializeField] private LevelData _initialLevel;
    [SerializeField] private LevelData _mainMenuLevel;

    private LevelData _currentLoadedLevel;
    public LevelData CurrentLoadedLevel => _currentLoadedLevel;

    private const string LEVEL_SPAWN_TAG = "LevelSpawnPoint";

    // On level loading trigger
    public delegate void OnLevelLoadingDelegate(bool isLevelLoading);
    public event OnLevelLoadingDelegate OnLevelLoadingChanged;

    private bool _isLevelLoading = false;
    public bool IsLevelLoading
    {
        get => _isLevelLoading;
        set
        {
            if (_isLevelLoading == value) return;
            _isLevelLoading = value;
            OnLevelLoadingChanged?.Invoke(IsLevelLoading);
        }
    }

    // Level loading percent trigger
    public delegate void OnLevelLoadPercentDelegate(float loadPercent);
    public event OnLevelLoadPercentDelegate OnLevelLoadPercentChanged;

    private float _levelLoadPercent = 0f;
    public float LevelLoadPercent
    {
        get => _levelLoadPercent;
        set
        {
            float v = Mathf.Clamp01(value);
            if (Mathf.Approximately(_levelLoadPercent, v)) return;
            _levelLoadPercent = v;
            OnLevelLoadPercentChanged?.Invoke(LevelLoadPercent);
        }
    }

    private int _loadingSyncTime = 1000;
    private int _finishLoadTime = 2000;

    public bool HasGameStarted { get; private set; } = false;

    public async Task InitalizeGame()
    {
        Logger.Started("Initializing game.");

        if (_initialLevel == null)
        {
            Logger.Error("Initial level not assinged.");
            return;
        }

        if (!_initialLevel.IsValid)
        {
            Logger.Error("LevelData invalid.");
            return;
        }

        MusicManager.I.Stop();
        PlayerManager.I.TogglePlayer(false);

        await Task.Delay(_loadingSyncTime);

        LevelLoadPercent = 0f;
        IsLevelLoading = true;

        await SceneManager.LoadSceneAsync(_initialLevel.SceneName, LoadSceneMode.Additive);
        _currentLoadedLevel = _initialLevel;

        var loadedScene = SceneManager.GetSceneByName(_currentLoadedLevel.SceneName);
        SceneManager.SetActiveScene(loadedScene);

        Logger.Processed("Initial level loaded successfully");

        LevelLoadPercent = 0.5f;

        PlayerManager.I.LoadPlayerOnLevel(GetSpawnPoint());

        Logger.Processed("Player set to level spawn point.");

        LevelLoadPercent = 1f;

        UiGlobalManager.I.TogglePauseGame(false);

        await Task.Delay(_finishLoadTime);
        
        PlayerManager.I.TogglePlayer(true);
        UiGlobalManager.I.ToggleHud(true);
        PlayerManager.I.SetPlayerLantern(_initialLevel.PlayerLanternIntensity);
        UiGlobalManager.I.Toggle(UiGlobalManager.I.MainMenu.gameObject, false);

        IsLevelLoading = false;
        HasGameStarted = true;

        if (_initialLevel.LevelMusic == null)
        {
            MusicManager.I.Stop();
        }
        else
        {
            MusicManager.I.Play(_initialLevel.LevelMusic);
        }

        Logger.Finalized("Game started with success.");
    }

    public async Task LoadLevel(LevelData levelToLoad)
    {
        Logger.Started("Initializing level loading.");

        if (levelToLoad == null)
        {
            Logger.Error("Level to load is null.");
            return;
        }

        if (!levelToLoad.IsValid)
        {
            Logger.Error("LevelData invalid.");
            return;
        }

        IsLevelLoading = true;
        LevelLoadPercent = 0f;

        MusicManager.I.Stop();
        PlayerManager.I.TogglePlayer(false);

        await Task.Delay(_loadingSyncTime);

        if (_currentLoadedLevel != null)
        {
            var scene = SceneManager.GetSceneByName(_currentLoadedLevel.SceneName);

            Light2D globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();

            if (globalLight != null)
            {
                Destroy(globalLight.gameObject);
                globalLight = null;
            }
            else
            {
                Logger.Error("Couldn't find GlobalLight to destroy.");
            }

            await Task.Delay(_loadingSyncTime);

            if (scene.isLoaded) await SceneManager.UnloadSceneAsync(scene);

            await Task.Delay(_loadingSyncTime);
        }

        LevelLoadPercent = 0.33f;

        await SceneManager.LoadSceneAsync(levelToLoad.SceneName, LoadSceneMode.Additive);
        _currentLoadedLevel = levelToLoad;

        var loadedScene = SceneManager.GetSceneByName(_currentLoadedLevel.SceneName);
        SceneManager.SetActiveScene(loadedScene);

        LevelLoadPercent = 0.67f;

        PlayerManager.I.LoadPlayerOnLevel(GetSpawnPoint());

        LevelLoadPercent = 1f;

        UiGlobalManager.I.TogglePauseGame(false);

        await Task.Delay(_finishLoadTime);

        PlayerManager.I.TogglePlayer(true);
        UiGlobalManager.I.ToggleHud(true);
        PlayerManager.I.SetPlayerLantern(levelToLoad.PlayerLanternIntensity);

        IsLevelLoading = false;
        HasGameStarted = true;

        if (levelToLoad.LevelMusic == null)
        {
            MusicManager.I.Stop();
        }
        else
        {
            MusicManager.I.Play(levelToLoad.LevelMusic);
        }

        Logger.Finalized($"Level ({levelToLoad.SceneName}) loaded with success.");
    }


    public async Task ReturnToMainMenu()
    {
        Logger.Started("Initializing return to menu.");


        if (_mainMenuLevel == null)
        {
            Logger.Error("Initial level not assinged.");
            return;
        }

        if (!_mainMenuLevel.IsValid)
        {
            Logger.Error("LevelData invalid.");
            return;
        }

        LevelLoadPercent = 0f;
        IsLevelLoading = true;

        MusicManager.I.Stop();
        PlayerManager.I.TogglePlayer(false);
        UiGlobalManager.I.ToggleHud(false);


        await Task.Delay(_loadingSyncTime);

        if (_currentLoadedLevel != null)
        {
            var scene = SceneManager.GetSceneByName(_currentLoadedLevel.SceneName);

            Light2D globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();

            if (globalLight != null)
            {
                Destroy(globalLight.gameObject);
                globalLight = null;
            }
            else
            {
                Logger.Error("Couldn't find GlobalLight to destroy.");
            }

            await Task.Delay(_loadingSyncTime);

            if (scene.isLoaded) await SceneManager.UnloadSceneAsync(scene);

            await Task.Delay(_loadingSyncTime);
        }

        LevelLoadPercent = 0.33f;

        var loadedScene = SceneManager.GetSceneByName(_mainMenuLevel.SceneName);
        SceneManager.SetActiveScene(loadedScene);

        LevelLoadPercent = 0.67f;

        LevelLoadPercent = 1f;
        await Task.Delay(_finishLoadTime);

        IsLevelLoading = false;
        HasGameStarted = false;

        UiGlobalManager.I.Toggle(UiGlobalManager.I.MainMenu.gameObject, true);
        PlayerManager.I.SetPlayerLantern(_mainMenuLevel.PlayerLanternIntensity);

        if (_mainMenuLevel.LevelMusic == null)
        {
            MusicManager.I.Stop();
        }
        else
        {
            MusicManager.I.Play(_mainMenuLevel.LevelMusic);
        }

        Logger.Finalized($"Returned to main menu with success.");
    }

    public Transform GetSpawnPoint()
    {
        GameObject go = GameObject.FindGameObjectWithTag(LEVEL_SPAWN_TAG);

        if (go != null)
        {
            return go.transform;
        }
        else
        {
            Logger.Waring("Spawn point not found. Using fallback at world origin.");
            GameObject fallback = new GameObject("SpawnPoint_Fallback");
            fallback.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            return fallback.transform;
        }
    }
}
