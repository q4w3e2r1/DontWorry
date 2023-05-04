using SQL_Quest.Creatures.Player;
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

        private static readonly int FadeAnim = Animator.StringToHash("Fade");

        public void Load()
        {
            _playerData.Position = _position;
            _playerData.InvertScale = _invertScale;
            _playerData.InteractOnStart = _interactOnStart;
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}