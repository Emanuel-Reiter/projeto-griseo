using TMPro;
using UnityEngine;

public class UiConfigMenu : UiBase
{
    private bool _isMenuOpen = false;

    [Header("Ui")]
    [SerializeField] private TMP_Text _sfxVolumePercent;
    [SerializeField] private TMP_Text _musicVolumePercent;

    private void Update()
    {
        if (_isMenuOpen)
        {
            if (PlayerManager.I.Deps.Input.Cancel.Pressed || PlayerManager.I.Deps.Input.PauseGame.Pressed)
            ToggleConfigMenu(false);
        }
    }

    public void ToggleConfigMenu(bool toggle)
    {
        if (UiGlobalManager.I.IsMenuTransitioning) return;

        if (LevelManager.I.HasGameStarted)
        {
            UiGlobalManager.I.Transition(UiGlobalManager.I.PauseMenu, !toggle, UiGlobalManager.I.BaseTransitionTime, () => { });
        }
        else
        {
            UiGlobalManager.I.Transition(UiGlobalManager.I.MainMenu, !toggle, UiGlobalManager.I.BaseTransitionTime, () => { });
        }

        UiGlobalManager.I.Transition(UiGlobalManager.I.ConfigMenu, toggle, UiGlobalManager.I.BaseTransitionTime, () => { });

        _isMenuOpen = toggle;
    }

    public void UpdateSfxVolumeText(float percent)
    {
        if (_sfxVolumePercent == null)
        {
            Logger.Waring("No sfx volume percent text assigned.");
            return;
        }

        _sfxVolumePercent.text = $"{Mathf.RoundToInt(percent * 100f)}%";
    }

    public void UpdateMusicVolumeText(float percent)
    {
        if (_musicVolumePercent == null)
        {
            Logger.Waring("No music volume percent text assigned.");
            return;
        }

        _musicVolumePercent.text = $"{Mathf.RoundToInt(percent * 100f)}%";
    }

    public void ChangeScreenMode(int mode)
    {
        Logger.Write($"{mode}");

        switch (mode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void ToggleVSync(bool toggle)
    {
        QualitySettings.vSyncCount = toggle ? 1 : 0;
    }
}
