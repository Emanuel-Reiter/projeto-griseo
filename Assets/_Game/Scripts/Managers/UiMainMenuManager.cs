using DG.Tweening;
using UnityEngine;

public class UiMainMenuManager : Singleton<UiMainMenuManager>
{
    [SerializeField] private CanvasGroup _mianMenu;

    private void Start()
    {

    }

    public void FadeToggle(bool toggle)
    {
        if (toggle)
        {
            Toggle(true);
            _mianMenu.alpha = 0f;
            _mianMenu.DOFade(1f, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                _mianMenu.alpha = 1f;
            });
        }
        else
        {
            _mianMenu.alpha = 1f;
            _mianMenu.DOFade(0f, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                _mianMenu.alpha = 0f;
                Toggle(false);
            });
        }
    }

    public void Toggle(bool toggle)
    {
        if (toggle) _mianMenu.gameObject.SetActive(true);
        else _mianMenu.gameObject.SetActive(false);
    }

    public void StarGame()
    {
        _ = LevelManager.I.InitalizeGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
