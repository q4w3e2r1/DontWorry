using System.Linq;
using TMPro;
using UnityEngine;

public class DropdownTemplateAutoLayout : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        var dropdownTemplateSizeDelta = new Vector2(dropdown.options.Select(opt => opt.text.Length).Max() * 12, dropdown.template.sizeDelta.y);
        dropdown.template.sizeDelta = dropdownTemplateSizeDelta;
        //dropdown.template.position = new Vector3(dropdownTemplateSizeDelta.x / 2, dropdown.template.position.y, dropdown.template.position.z);
    }
}
