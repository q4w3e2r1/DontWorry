using UnityEngine;
using UnityEngine.SceneManagement;

namespace SQL_Quest.Components.LevelManagement
{
    public class LoadLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void Load()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}