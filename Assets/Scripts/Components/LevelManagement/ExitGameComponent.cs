using UnityEngine;

namespace SQL_Quest.Components.LevelManagement
{
    public class ExitGameComponent : MonoBehaviour
    {
        public void Exit()
        {
            Application.Quit();
        }
    }
}