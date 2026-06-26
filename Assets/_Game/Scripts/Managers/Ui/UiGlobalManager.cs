using DG.Tweening;
using System;
using System.Collections.Generic;
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
    [SerializeField] private Canvas _uiBgBaseCanvas;

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

    private UiBase _uiBgBase;
    public UiBase UiBgBase => _uiBgBase;


    // Game related flags
    public bool IsGamePaused { get; private set; } = false;

    public bool HasMenuActive = true;
    public bool IsMenuTransitioning { get; private set; } = false;

    // Transition params
    public float BaseTransitionTime { get; private set; } = 0.25f;

    // Button params
    public float BtnDisabledOpacity { get; private set; } = 0.2f;


    private Stack<UiBase> _menuStack = new Stack<UiBase>();


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

        Toggle(_uiBgBaseCanvas.gameObject, true);
        _uiBgBase = GetUiRefs(_uiBgBaseCanvas.gameObject);
    }

    private void Start()
    {
        Toggle(MainMenu.CanvasGroup.gameObject, true);
        Toggle(PauseMenu.CanvasGroup.gameObject, false);
        Toggle(ConfigMenu.CanvasGroup.gameObject, false);
        Toggle(LoadingScreen.CanvasGroup.gameObject, false);
        Toggle(Hud.CanvasGroup.gameObject, false);
        Toggle(UiBgBase.CanvasGroup.gameObject, true);

        SubscribeToMethods();
    }

    private void Update()
    {
        if (PlayerManager.I.Deps.Input.PauseGame.Pressed)
        {
            TogglePauseGame(true);
        }

        if (PlayerManager.I.Deps.Input.Cancel.Pressed)
        {
            if (_menuStack.Count == 1) TogglePauseGame(false);
            CloseActiveMenu();
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

        Logger.Write($"Toggling {(toggle ? "ON" : "OFF")} {target.name}");

        target.SetActive(toggle);
    }


    public void NavigatgeToMenu(UiBase targetMenu, float transitionTime, Action callback)
    {
        if (IsMenuTransitioning) return;

        Logger.Started($"Navigating to {targetMenu.name}");

        EventSystem.current.SetSelectedGameObject(null);

        if (targetMenu == null)
        {
            Logger.Error($"Menu reference not assigned correctly.");
            return;
        }

        IsMenuTransitioning = true;

        Logger.Processed($"Menu references ok.");

        Toggle(targetMenu.CanvasGroup.gameObject, true);
        targetMenu.CanvasGroup.alpha = 0f;
        targetMenu.CanvasGroup.DOFade(1f, transitionTime).SetUpdate(true).OnComplete(() =>
        {
            targetMenu.CanvasGroup.alpha = 1f;
            callback?.Invoke();
            IsMenuTransitioning = false;

            Logger.Finalized($"Menu loaded ok.");
        });

        
        if (_menuStack.Count > 0)
        {
            UiBase currentMenu = _menuStack.Peek();
            currentMenu.SetIsActive(false);
            Fade(currentMenu.CanvasGroup, false, BaseTransitionTime);

            Logger.Processed($"Fade previous menu");
        }

        _menuStack.Push(targetMenu);
        targetMenu.SetIsActive(true);

        Logger.Processed($"Add menu to stack and activated it.");
    }

    public void CloseActiveMenu()
    {
        if (IsMenuTransitioning) return;

        if (_menuStack.Count <= 1) return;

        EventSystem.current.SetSelectedGameObject(null);

        UiBase currentMenu = _menuStack.Pop();
        currentMenu.SetIsActive(false);
        Fade(currentMenu.CanvasGroup, false, BaseTransitionTime);
    }

    public void Fade(CanvasGroup cg, bool toggle, float transitionTime)
    {
        if (cg == null)
        {
            Logger.Error($"UI Element reference not assigned correctly.");
            return;
        }

        if (toggle)
        {
            Toggle(cg.gameObject, true);
            cg.alpha = 0f;
            cg.DOFade(1f, transitionTime).SetUpdate(true).OnComplete(() =>
            {
                cg.alpha = 1f;
            });
        }
        else
        {
            cg.alpha = 1f;
            cg.DOFade(0f, transitionTime).SetUpdate(true).OnComplete(() =>
            {
                cg.alpha = 0f;
                Toggle(cg.gameObject, false);
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
        Fade(LoadingScreen.CanvasGroup, toggle, BaseTransitionTime);
    }

    public void StartGame()
    {
        _ = LevelManager.I.InitalizeGame();
        Fade(UiBgBase.CanvasGroup, false, BaseTransitionTime);
    }

    public void ReturnToMainMenu()
    {
        _ = LevelManager.I.ReturnToMainMenu();
        Fade(UiBgBase.CanvasGroup, true, BaseTransitionTime);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToggleHud(bool toggle)
    {
        Fade(Hud.CanvasGroup, toggle, BaseTransitionTime);
    }

    public void TogglePauseGame(bool toggle)
    {
        if (IsMenuTransitioning) return;

        if (toggle)
        {
            IsGamePaused = true;
            Time.timeScale = 0f;
            PlayerManager.I.TogglePlayer(false);

            // Disable hud when pause menu is on
            ToggleHud(false);

            NavigatgeToMenu(PauseMenu, BaseTransitionTime, () => { });
        }
        else
        {
            IsGamePaused = false;
            Time.timeScale = 1f;
            PlayerManager.I.TogglePlayer(true);

            // Enable hud when pause menu is off
            ToggleHud(true);
        }
    }
    #endregion
}