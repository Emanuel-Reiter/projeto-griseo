using TMPro;
using UnityEngine;

public class UiConfigMenu : UiBase
{
    [Header("Ui")]
    [SerializeField] private TMP_Text _sfxVolumePercent;
    [SerializeField] private TMP_Text _musicVolumePercent;

    public void OpenConfigMenu()
    {
        UiGlobalManager.I.NavigatgeToMenu(UiGlobalManager.I.PauseMenu, UiGlobalManager.I.BaseTransitionTime, () => { });
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
