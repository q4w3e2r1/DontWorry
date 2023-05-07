using SQL_Quest.Components.UI;
using System.Collections;
using UnityEngine;

namespace SQL_Quest.Components.LevelManagement
{
    public class ExitGameComponent : MonoBehaviour
    {
        public void Exit()
        {
            StartCoroutine(ExitGameRoutine());
        }

        private IEnumerator ExitGameRoutine()
        {
            var waitFading = true;
            Fader.Instance.FadeIn(() => waitFading = false);
            yield return new WaitUntil(() => waitFading == false);

            Application.Quit();
        }
    }
}