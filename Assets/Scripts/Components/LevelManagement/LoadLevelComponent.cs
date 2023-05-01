using SQL_Quest.Creatures.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SQL_Quest.Components.LevelManagement
{
    public class LoadLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneToLoad;
        [SerializeField] private Vector3 _position;
        [SerializeField] private PlayerData _playerData;

        public void Load()
        {
            _playerData.Position = _position;
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}