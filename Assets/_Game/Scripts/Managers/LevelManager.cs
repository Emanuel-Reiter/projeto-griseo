using System.Threading.Tasks;
using UnityEngine;
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

    private int _playerAnimSyncTime = 1000;
    private int _finishLoadTime = 2000;

    public bool HasGameStarted { get; private set; } = false;

    public async Task InitalizeGame()
    {
        if (_initialLevel == null)
        {
            Debug.LogError("Initial level not assinged.");
            return;
        }

        if (!_initialLevel.IsValid)
        {
            Debug.LogError("LevelData invalid.");
            return;
        }

        MusicManager.I.Stop();

        await Task.Delay(_playerAnimSyncTime);

        //UIManager.I.InitializeMenus();

        LevelLoadPercent = 0f;
        IsLevelLoading = true;

        //try
        //{
        //    await GameManager.I.InitializeGame();
        //}
        //catch (InitializationException e)
        //{
        //    Debug.LogError($"Game initialization error. {e}");
        //    return;
        //}

        //GameManager.I.TogglePlayerMovement(false);

        //GameContext.I.LoadPlayerRefs();
        //UIManager.I.InitializeInteractPrompt();
        //UIManager.I.InitializeHUD();

        await SceneManager.LoadSceneAsync(_initialLevel.SceneName, LoadSceneMode.Additive);
        _currentLoadedLevel = _initialLevel;

        var loadedScene = SceneManager.GetSceneByName(_currentLoadedLevel.SceneName);
        SceneManager.SetActiveScene(loadedScene);

        LevelLoadPercent = 0.5f;

        //GameManager.I.MovePlayerToSpawn(GetSpawnPoint());

        LevelLoadPercent = 1f;
        await Task.Delay(_finishLoadTime);

        IsLevelLoading = false;
        HasGameStarted = true;
        //LevelProgressManager.I.ResetProgress();
        //GameContext.I.PlayerAttributes.Heal(99999);
        //GameManager.I.TogglePlayerMovement(true);

        if (_initialLevel.LevelMusic == null)
        {
            MusicManager.I.Stop();
        }
        else
        {
            MusicManager.I.Play(_initialLevel.LevelMusic);
        }
    }

    public async Task LoadLevel(LevelData levelToLoad)
    {
        if (levelToLoad == null)
        {
            Debug.LogError("Level to load is null.");
            return;
        }

        if (!levelToLoad.IsValid)
        {
            Debug.LogError("LevelData invalid.");
            return;
        }

        IsLevelLoading = true;
        LevelLoadPercent = 0f;

        MusicManager.I.Stop();

        // GameManager.I.TogglePlayerMovement(false);

        await Task.Delay(_playerAnimSyncTime);

        if (_currentLoadedLevel != null)
        {
            var scene = SceneManager.GetSceneByName(_currentLoadedLevel.SceneName);
            if (scene.isLoaded) await SceneManager.UnloadSceneAsync(scene);
        }

        LevelLoadPercent = 0.33f;

        await SceneManager.LoadSceneAsync(levelToLoad.SceneName, LoadSceneMode.Additive);
        _currentLoadedLevel = levelToLoad;

        var loadedScene = SceneManager.GetSceneByName(_currentLoadedLevel.SceneName);
        SceneManager.SetActiveScene(loadedScene);

        LevelLoadPercent = 0.67f;

       //GameManager.I.MovePlayerToSpawn(GetSpawnPoint());

        LevelLoadPercent = 1f;
        await Task.Delay(_finishLoadTime);

        IsLevelLoading = false;
        HasGameStarted = true;
        
        // Respawn logic
        //LevelProgressManager.I.ResetProgress();
        //GameContext.I.PlayerAttributes.Heal(99999);
        //GameContext.I.ResetCinemachineLookRotation();
        //GameManager.I.TogglePlayerMovement(true);

        if (levelToLoad.LevelMusic == null)
        {
            MusicManager.I.Stop();
        }
        else
        {
            MusicManager.I.Play(levelToLoad.LevelMusic);
        }
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
            Debug.LogWarning("Spawn point not found. Using fallback at world origin.");
            GameObject fallback = new GameObject("SpawnPoint_Fallback");
            fallback.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            return fallback.transform;
        }
    }
}
