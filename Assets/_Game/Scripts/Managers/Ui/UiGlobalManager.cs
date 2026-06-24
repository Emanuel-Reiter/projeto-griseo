using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiGlobalManager : Singleton<UiGlobalManager>
{
    [Header("Canvas refs")]
    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private Canvas _pauseMenuCanvas;
    [SerializeField] private Canvas _configMenuCanvas;
    [SerializeField] private Canvas _loadingScreenCanvas;
    [SerializeField] private Canvas _hudCanvas;

    private UiBase _mainMenu;
    public UiBase MainMenu => _mainMenu;

    private UiBase _pauseMenu;
    public UiBase PauseMenu => _pauseMenu;

    private UiBase _configMenu;
    public UiBase ConfigMenu => _configMenu;

    private UiBase _loadingScreen;
    public UiBase LoadingScreen => _loadingScreen;

    private UiBase _hud;
    public UiBase Hud => _hud;

    // Game related flags
    public bool IsGamePaused { get; private set; } = false;

    public bool HasMenuActive = true;
    public bool OnPauseMenuTransition { get; private set; } = false;

    // Transition params
    public float BaseTransitionTime { get; private set; } = 0.25f;

    protected override void Awake()
    {
        base.Awake();

        InitializeGameUi();
    }

    private void InitializeGameUi() 
    {
        Toggle(_mainMenuCanvas.gameObject, true);
        _mainMenu = GetUiRefs(_mainMenuCanvas.gameObject);

        Toggle(_pauseMenuCanvas.gameObject, true);
        _pauseMenu = GetUiRefs(_pauseMenuCanvas.gameObject);

        Toggle(_configMenuCanvas.gameObject, true);
        _configMenu = GetUiRefs(_configMenuCanvas.gameObject);

        Toggle(_loadingScreenCanvas.gameObject, true);
        _loadingScreen = GetUiRefs(_loadingScreenCanvas.gameObject);

        Toggle(_hudCanvas.gameObject, true);
        _hud = GetUiRefs(_hudCanvas.gameObject);
    }

    private void Start()
    {
        Toggle(_mainMenu.CanvasGroup.gameObject, true);
        Toggle(_pauseMenu.CanvasGroup.gameObject, false);
        Toggle(_configMenu.CanvasGroup.gameObject, false);
        Toggle(_loadingScreen.CanvasGroup.gameObject, false);
        Toggle(_hud.CanvasGroup.gameObject, false);

        SubscribeToMethods();
    }

    private void Update()
    {
        if (!IsGamePaused && PlayerManager.I.Deps.Input.PauseGame.Pressed)
        {
            TogglePauseGame(true);
        }

        if (IsGamePaused && PlayerManager.I.Deps.Input.Cancel.Pressed)
        {
            TogglePauseGame(false);
        }
    }

    #region Toggles and Transitions
    public void Toggle(GameObject target, bool toggle)
    {
        if (target == null)
        {
            Logger.Error($"UI Element reference not assigned correctly.");
            return;
        }

        target.SetActive(toggle);
    }

    public void Transition(UiBase target, bool toggle, float transitionTime, Action callback)
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (target == null)
        {
            Logger.Error($"UI Element reference not assigned correctly.");
            return;
        }

        if (toggle)
        {
            Toggle(target.CanvasGroup.gameObject, true);
            target.CanvasGroup.alpha = 0f;
            target.CanvasGroup.DOFade(1f, transitionTime).SetUpdate(true).OnComplete(() =>
            {
                target.CanvasGroup.alpha = 1f;
                callback?.Invoke();
            });
        }
        else
        {
            target.CanvasGroup.alpha = 1f;
            target.CanvasGroup.DOFade(0f, transitionTime).SetUpdate(true).OnComplete(() =>
            {
                target.CanvasGroup.alpha = 0f;
                Toggle(target.CanvasGroup.gameObject, false);
                callback?.Invoke();
            });
        }
    }
    #endregion

    #region References and Loaders
    private UiBase GetUiRefs(GameObject parentGo)
    {
        if (parentGo == null)
        {
            Logger.Error($"Canvas GameObject is null.");
            return null;
        }

        UiBase UiRef = parentGo.GetComponentInChildren<UiBase>();

        if (UiRef == null)
        {
            Logger.Error($"Canvas Group not found in parent GameObject.");
            return null;
        }
        else
        {
            return UiRef;
        }
    }

    private void SubscribeToMethods()
    {
        LevelManager.I.OnLevelLoadingChanged += ManageLoadScreen;
    }
    #endregion

    #region Ui Methods
    private void ManageLoadScreen(bool toggle)
    {
        Logger.Write("Tiggered loading screen.");
        Transition(LoadingScreen, toggle, BaseTransitionTime, () => { });
    }

    public void StarGame()
    {
        _ = LevelManager.I.InitalizeGame();
    }

    public void ReturnToMainMenu()
    {
        _ = LevelManager.I.ReturnToMainMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToggleHud(bool toggle)
    {
        Transition(Hud, toggle, BaseTransitionTime, () => { });
    }

    public void TogglePauseGame(bool toggle)
    {
        // Return if OnPauseMenuTransition is true to avoid breaking the transition logic
        if (OnPauseMenuTransition) return;

        if (toggle)
        {
            IsGamePaused = true;
            Time.timeScale = 0f;
            PlayerManager.I.TogglePlayer(false);

            // Disable hud when pause menu is on
            ToggleHud(false);

            OnPauseMenuTransition = true;
            Transition(PauseMenu, true, BaseTransitionTime, () => OnPauseMenuTransition = false);
        }
        else
        {
            IsGamePaused = false;
            Time.timeScale = 1f;
            PlayerManager.I.TogglePlayer(true);

            // Enable hud when pause menu is off
            ToggleHud(true);

            OnPauseMenuTransition = true;
            Transition(PauseMenu, false, BaseTransitionTime, () => OnPauseMenuTransition = false);
        }
    }
    #endregion

}

