using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Components.UI.Dialogs
{
    public class DialogContent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _icon;

        public TextMeshProUGUI Name => _name;
        public TextMeshProUGUI Text => _text;

        public void TrySetIcon(Sprite icon)
        {
            if (icon != null)
                _icon.sprite = icon;
        }
    }
}