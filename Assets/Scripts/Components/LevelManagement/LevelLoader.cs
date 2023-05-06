using SQL_Quest.Components.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Components.LevelManagement
{
    public class LevelLoader : MonoBehaviour
    {
        private const string LEVELLOADER_PATH = "LevelLoader";

        private static LevelLoader _instance;

        public static LevelLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<LevelLoader>(LEVELLOADER_PATH);
                    _instance = Instantiate(prefab);
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        public void Load(string sceneToLoad)
        { 
            StartCoroutine(LoadSceneRoutine(sceneToLoad));
        }

        private IEnumerator LoadSceneRoutine(string sceneToLoad)
        {
            var waitFading = true;
            Fader.Instance.FadeIn(() => waitFading = false);

            yield return new WaitUntil(() => waitFading == false);

            SceneManager.LoadScene(sceneToLoad);

            waitFading = true;
            Fader.Instance.FadeOut(() => waitFading = false);

            yield return new WaitUntil(() => waitFading == false);
        }
    }
}
