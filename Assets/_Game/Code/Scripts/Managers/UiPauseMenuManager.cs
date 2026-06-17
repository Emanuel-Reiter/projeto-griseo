using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiPauseMenuManager : Singleton<UiPauseMenuManager>
{
    [SerializeField] private CanvasGroup _pauseMenu;

    public bool IsGamePaused { get; private set; } = false;

    public bool OnPauseMenuTransition { get; private set; } = false;

    private void Start()
    {
        Toggle(false);
        TimerManager.I.StartTimer(0.1f, () => { TogglePauseGame(true); });
    }

    private void Update()
    {
        if (PlayerManager.I.Deps.Input.PauseGame.Pressed)
        {
            TogglePauseGame(true);
        }

        if (IsGamePaused)
        {
            if (PlayerManager.I.Deps.Input.Cancel.Pressed)
            {
                TogglePauseGame(false);
            }
        }
    }

    public void FadeToggle(bool toggle)
    {
        if (toggle)
        {
            OnPauseMenuTransition = true;
            Toggle(true);
            _pauseMenu.alpha = 0f;
            _pauseMenu.DOFade(1f, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                _pauseMenu.alpha = 1f;
                OnPauseMenuTransition = false;
            });
        }
        else
        {
            OnPauseMenuTransition = true;
            _pauseMenu.alpha = 1f;
            _pauseMenu.DOFade(0f, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                _pauseMenu.alpha = 0f;
                Toggle(false);
                OnPauseMenuTransition = false;
            });
        }
    }

    public void Toggle(bool toggle)
    {
        if (toggle) _pauseMenu.gameObject.SetActive(true);
        else _pauseMenu.gameObject.SetActive(false);
    }

    public void TogglePauseGame(bool toggle)
    {
        if (OnPauseMenuTransition) return;

        if (toggle)
        {
            Time.timeScale = 0f;
            FadeToggle(true);
            IsGamePaused = true;
            PlayerManager.I.TogglePlayer(false);
        }
        else
        {
            Time.timeScale = 1f;
            FadeToggle(false);
            IsGamePaused = false;
            PlayerManager.I.TogglePlayer(true);
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ReturnToMainMenu()
    {
        _ = LevelManager.I.ReturnToMainMenu();
    }
}
