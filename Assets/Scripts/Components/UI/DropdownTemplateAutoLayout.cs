using System.Linq;
using TMPro;
using UnityEngine;

namespace SQL_Quest.Components.UI
{
    public class DropdownTemplateAutoLayout : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;

        private void Start()
        {
            var dropdownTemplateSizeDelta = new Vector2(dropdown.options.Select(opt => opt.text.Length).Max() * 12, dropdown.template.sizeDelta.y);
            dropdown.template.sizeDelta = dropdownTemplateSizeDelta;
        }
    }
}