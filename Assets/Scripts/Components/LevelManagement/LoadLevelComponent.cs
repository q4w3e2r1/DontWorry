﻿using SQL_Quest.Components.UI;
using SQL_Quest.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SQL_Quest.Components.LevelManagement
{
    public class LoadLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneToLoad;
        [SerializeField] private UnityEvent _onLoad;
        [Space]
        [SerializeField] private bool _isNextLevel;
        [SerializeField] private Vector3 _position;
        [SerializeField] private bool _invertScale;
        [SerializeField] private bool _interactOnStart;

        public void Load()
        {
            StartCoroutine(LoadSceneRoutine());
        }

        private IEnumerator LoadSceneRoutine()
        {
            var waitFading = true;
            Fader.Instance.FadeIn(() => waitFading = false);

            yield return new WaitUntil(() => waitFading == false);

            _onLoad?.Invoke();
            SetPlayerPrefs();

            if (PlayerPrefs.GetInt("LevelNumber") <= 5)
                SceneManager.LoadScene(_sceneToLoad);
            else
                SceneManager.LoadScene("MainMenu");

            waitFading = true;
            Fader.Instance.FadeOut(() => waitFading = false);

            yield return new WaitUntil(() => waitFading == false);
        }

        private void SetPlayerPrefs()
        {
            PlayerPrefsExtensions.SetVector3("Position", _position);
            PlayerPrefsExtensions.SetBool("InvertScale", _invertScale);
            PlayerPrefsExtensions.SetBool("InteractOnStart", _interactOnStart);
            if (_isNextLevel)
                PlayerPrefs.SetInt("LevelNumber", PlayerPrefs.GetInt("LevelNumber") + 1);
        }
    }
}