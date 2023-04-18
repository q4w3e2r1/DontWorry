using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Extensions
{
    public static class TMP_DropdownExtensions
    {
        public static void SetOptions(this TMP_Dropdown dropdown, string[] strings)
        {
            var options = strings.Select(s =>
            {
                var option = new TMP_Dropdown.OptionData();
                option.text = s;
                return option;
            }).ToList();

            var emptyOption = new TMP_Dropdown.OptionData();
            emptyOption.text = "...";
            options.Insert(0, emptyOption);

            dropdown.ClearOptions();
            dropdown.AddOptions(options);
        }
    }
}