using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SQL_Quest.Components.UI.Shell
{
    public class Modifier : MonoBehaviour
    {
        public void AddListeners(UnityAction onClickAddButton, UnityAction onClickDestroyButton)
        {
            var buttons = GetComponentsInChildren<Button>();
            foreach (var button in buttons)
                button.onClick.RemoveAllListeners();
            buttons[0].onClick.AddListener(onClickAddButton);
            buttons[1].onClick.AddListener(onClickDestroyButton);
        }
    }
}