using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiPauseMenuManager : Singleton<UiPauseMenuManager>
{
    [SerializeField] private CanvasGroup _pauseMenu;

    public bool IsGamePaused { get; private set; } = false;

    private void Start()
    {
        Toggle(false);
    }

    public void FadeToggle(bool toggle)
    {
        if (toggle)
        {
            _pauseMenu.gameObject.SetActive(true);
            _pauseMenu.alpha = 0f;
            _pauseMenu.DOFade(1f, 0.25f).OnComplete(() =>
            {
                _pauseMenu.alpha = 1f;
            });
        }
        else
        {
            _pauseMenu.alpha = 1f;
            _pauseMenu.DOFade(0f, 0.25f).OnComplete(() =>
            {
                _pauseMenu.alpha = 0f;
                _pauseMenu.gameObject.SetActive(false);
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
        if (toggle)
        {
            TimerManager.I.StartTimer(0.3f, () => { Time.timeScale = 0f; });
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
}
