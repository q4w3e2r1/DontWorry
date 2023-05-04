using SQL_Quest.Components.UI;
using SQL_Quest.Creatures.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SQL_Quest.Components.LevelManagement
{
    public class LoadLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneToLoad;
        [SerializeField] private Vector3 _position;
        [SerializeField] private bool _invertScale;
        [SerializeField] private bool _interactOnStart;
        [SerializeField] private PlayerData _playerData;

        private bool _isLoading;

        public void Load()
        {
            if (_isLoading)
                return;

            _playerData.Position = _position;
            _playerData.InvertScale = _invertScale;
            _playerData.InteractOnStart = _interactOnStart;
            StartCoroutine(LoadSceneRoutine());
        }

        private IEnumerator LoadSceneRoutine()
        {
            _isLoading = true;

            var waitFading = true;
            Fader.Instance.FadeIn(() => waitFading = false);

            while (waitFading)
                yield return null;

            SceneManager.LoadScene(_sceneToLoad);

            waitFading = true;
            Fader.Instance.FadeOut(() => waitFading = false);

            while (waitFading)
                yield return null;

            _isLoading = false;
        }
    }
}