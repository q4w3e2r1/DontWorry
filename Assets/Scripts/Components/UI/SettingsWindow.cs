using SQL_Quest.Components.Audio;
using SQL_Quest.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Components.UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private Slider _soundVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [Space]
        [SerializeField] private TMP_Dropdown _windowResolutionDropdown;
        [SerializeField] private Toggle _windowfullScreenToggle;
        [SerializeField] private Toggle _windowDarkThemeToggle;

        private int _resolutionIndex;
        private bool _isFullScreen;
        private bool _isDarkTheme;

        private Resolution[] resolutions;

        private void OnEnable()
        {
            if (PlayerPrefs.HasKey("SoundVolume") && PlayerPrefs.HasKey("MusicVolume"))
            {
                _soundVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume");
                _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            }

            _windowResolutionDropdown.ClearOptions();

            resolutions = Screen.resolutions.Reverse().ToArray();
            var options = new List<string>();
            for (int i = 0; i < resolutions.Length; i++)
            {
                var option = $"{resolutions[i].width} x {resolutions[i].height} {resolutions[i].refreshRate} HZ";
                options.Add(option);
            }
            _windowResolutionDropdown.AddOptions(options);
            _windowResolutionDropdown.value = _resolutionIndex;

            _windowfullScreenToggle.isOn = Screen.fullScreen;

            if (!PlayerPrefs.HasKey("DarkTheme"))
                PlayerPrefsExtensions.SetBool("DarkTheme", false);
            _windowDarkThemeToggle.isOn = PlayerPrefsExtensions.GetBool("DarkTheme");
        }

        public void SetResolution(int resolutionIndex)
        {
            _resolutionIndex = resolutionIndex;
        }

        public void SetFullScreen(bool isFullScreen)
        {
            _isFullScreen = isFullScreen;
        }

        public void SetDarkTheme(bool isDarkTheme)
        {
            _isDarkTheme = isDarkTheme;
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetFloat("SoundVolume", _soundVolumeSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", _musicVolumeSlider.value);

            PlayerPrefs.SetInt("ResolutionPreference", _resolutionIndex);
            PlayerPrefsExtensions.SetBool("FullScreenPreference", _isFullScreen);
            PlayerPrefsExtensions.SetBool("DarkTheme", _isDarkTheme);

            LoadSettings();
        }

        private void LoadSettings()
        {
            var resolution = resolutions[PlayerPrefs.GetInt("ResolutionPreference")];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);

            Screen.fullScreen = PlayerPrefsExtensions.GetBool("FullScreenPreference");
        }
    }
}
