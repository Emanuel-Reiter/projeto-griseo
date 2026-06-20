using DG.Tweening;
using UnityEngine;

public class UiHudManager : Singleton<UiHudManager>
{
    [SerializeField] private CanvasGroup _containerHud;

    private void Start()
    {
        Toggle(false);
    }

    public void FadeToggle(bool toggle)
    {
        if (toggle)
        {   
            _containerHud.gameObject.SetActive(true);
            _containerHud.alpha = 0f;
            _containerHud.DOFade(1f, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                _containerHud.alpha = 1f;
            });
        }
        else
        {
            _containerHud.alpha = 1f;
            _containerHud.DOFade(0f, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                _containerHud.alpha = 0f;
                _containerHud.gameObject.SetActive(false);
            });
        }
    }

    public void Toggle(bool toggle)
    {
        if (toggle) _containerHud.gameObject.SetActive(true);
        else _containerHud.gameObject.SetActive(false);
    }
}
