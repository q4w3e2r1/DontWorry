using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
            foreach(var gameObject in _commandDescriptionsHandler.GetComponentsInChildren<CommandDescription>())
                gameObject.gameObject.SetActive(false);
        }
    }
}
