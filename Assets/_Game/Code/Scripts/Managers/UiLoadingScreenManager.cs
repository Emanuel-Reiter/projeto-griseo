using DG.Tweening;
using UnityEngine;

public class UiLoadingScreenManager : Singleton<UiLoadingScreenManager>
{
    [SerializeField] private CanvasGroup _loadingScreen;

    private void Start()
    {
        Toggle(false);

        LevelManager.I.OnLevelLoadingChanged += FadeToggle;
    }

    public void FadeToggle(bool toggle)
    {

        if (toggle)
        {   
            _loadingScreen.gameObject.SetActive(true);
            _loadingScreen.alpha = 0f;
            _loadingScreen.DOFade(1f, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                _loadingScreen.alpha = 1f;
            });
        }
        else
        {
            _loadingScreen.alpha = 1f;
            _loadingScreen.DOFade(0f, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                _loadingScreen.alpha = 0f;
                _loadingScreen.gameObject.SetActive(false);
            });
        }
    }

    public void Toggle(bool toggle)
    {
        if (toggle) _loadingScreen.gameObject.SetActive(true);
        else _loadingScreen.gameObject.SetActive(false);
    }
}
