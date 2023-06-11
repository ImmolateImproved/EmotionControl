using UnityEngine;
using System.Linq;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Resolution[] resolutions;

    private FullScreenMode[] fullScreenModes;

    private int selectedFullScreenMode;
    private int selectedResolution;

    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private TMP_Dropdown fullScreenModeDropdown;

    [SerializeField] private BackgroundMusicManager musicManager;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        resolutions = Screen.resolutions;

        var resolutionStrings = resolutions.Select(x => x.ToString()).ToList();
        resolutionsDropdown.AddOptions(resolutionStrings);

        fullScreenModes = Enum.GetValues(typeof(FullScreenMode)).Cast<FullScreenMode>().ToArray();

        var fullScreenModeStrings = fullScreenModes.Select(x => x.ToString()).ToList();

        fullScreenModeDropdown.AddOptions(fullScreenModeStrings);

        resolutionsDropdown.SetValueWithoutNotify(resolutions.Length - 1);
        fullScreenModeDropdown.SetValueWithoutNotify(0);
    }

    public void StartGame()
    {
        musicManager.StartMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnResolutionChange(int index)
    {
        selectedResolution = index;

        var reso = resolutions[index];

        Screen.SetResolution(reso.width, reso.height, fullScreenModes[selectedFullScreenMode]);
    }

    public void OnFullScreenModeChange(int index)
    {
        selectedFullScreenMode = index;

        var reso = resolutions[selectedResolution];

        Screen.SetResolution(reso.width, reso.height, fullScreenModes[index]);
    }
}