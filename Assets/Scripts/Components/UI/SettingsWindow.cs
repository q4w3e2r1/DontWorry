﻿using SQL_Quest.Components.UI;
using SQL_Quest.Extensions;
using SQL_Quest.Extentions;
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
            _soundVolumeSlider.value = AudioController.Instance.SoundAudioSource.volume;
            _musicVolumeSlider.value = AudioController.Instance.MusicAudioSource.volume;

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
            AudioController.Instance.SoundAudioSource.volume = _soundVolumeSlider.value;
            AudioController.Instance.MusicAudioSource.volume = _musicVolumeSlider.value;

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
