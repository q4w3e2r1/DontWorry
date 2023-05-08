using SQL_Quest.Components.UI;
using SQL_Quest.Creatures.Player;
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
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Vector3 _position;
        [SerializeField] private bool _invertScale;
        [SerializeField] private bool _interactOnStart;

        public void Load()
        {
            _onLoad?.Invoke();

            _playerData.Position = _position;
            _playerData.InvertScale = _invertScale;
            _playerData.InteractOnStart = _interactOnStart;

            StartCoroutine(LoadSceneRoutine());
        }

        private IEnumerator LoadSceneRoutine()
        {
            var waitFading = true;
            Fader.Instance.FadeIn(() => waitFading = false);

            yield return new WaitUntil(() => waitFading == false);

            SceneManager.LoadScene(_sceneToLoad);

            waitFading = true;
            Fader.Instance.FadeOut(() => waitFading = false);

            yield return new WaitUntil(() => waitFading == false);
        }
    }
}