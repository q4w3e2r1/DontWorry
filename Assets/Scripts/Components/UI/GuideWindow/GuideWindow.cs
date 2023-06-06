using UnityEngine;

namespace SQL_Quest.Components.UI.GuideWindow
{
    public class GuideWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _commandDescriptionsHandler;

        private void Start()
        {
            foreach (var gameObject in _commandDescriptionsHandler.GetComponentsInChildren<CommandDescription>(true))
                gameObject.gameObject.SetActive(true);
            DisableAllDescriptions();
        }

        public void DisableAllDescriptions()
        {
            foreach (var gameObject in _commandDescriptionsHandler.GetComponentsInChildren<CommandDescription>())
                gameObject.gameObject.SetActive(false);
        }
    }
}
