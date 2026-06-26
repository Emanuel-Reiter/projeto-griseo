using UnityEngine;

public abstract class UiBase: MonoBehaviour
{
    [Header("Ui type")]
    public UiType UiType = UiType.Menu;

    private CanvasGroup _canvasGroup;
    public CanvasGroup CanvasGroup => _canvasGroup;

    public bool IsActive { get; private set; } = false;
    public void SetIsActive(bool active) { IsActive = active; }

    private void Awake()
    {
        _canvasGroup = GetComponentInChildren<CanvasGroup>();

        if (_canvasGroup == null)
        {
            Logger.Error($"No CanvasGroup found in children of UiBase GameObject.");
        }
    }
}

public enum UiType
{
    Menu,
    Hud,
}
