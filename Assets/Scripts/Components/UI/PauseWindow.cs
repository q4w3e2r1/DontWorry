using UnityEngine;

namespace SQL_Quest.Components.UI
{
    public class PauseWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseWindows;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchState();
            }
        }

        public void SwitchState()
        {
            _pauseWindows.SetActive(!_pauseWindows.activeSelf);
        }
    }
}