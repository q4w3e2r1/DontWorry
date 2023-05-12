using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine.Events;

namespace SQL_Quest.Extentions
{
    public static class TMP_DropdownExtensions
    {
        public static string Text(this TMP_Dropdown dropdown)
            => dropdown.captionText.text;

        public static bool IsEmpty(this TMP_Dropdown dropdown)
            => dropdown.Text() == "...";

        public static void SetOptions(this TMP_Dropdown dropdown, string[] strings, bool haveAllOption = false)
        {
            var options = strings.Select(s =>
            {
                var option = new TMP_Dropdown.OptionData
                {
                    text = s
                };
                return option;
            }).ToList();

            var emptyOption = new TMP_Dropdown.OptionData
            {
                text = "..."
            };
            options.Insert(0, emptyOption);

            if (haveAllOption)
            {
                var allOption = new TMP_Dropdown.OptionData
                {
                    text = "*"
                };
                options.Insert(1, allOption);
            }

            dropdown.ClearOptions();
            dropdown.AddOptions(options);
        }

        public static void SetOptions(this TMP_Dropdown dropdown, List<string> strings, bool haveAllOption = false)
        {
            strings.ToArray();
            dropdown.SetOptions(strings, haveAllOption);
        }

        public static void AddListeners(this TMP_Dropdown dropdown, params UnityAction<int>[] listeners)
        {
            foreach(var listener in listeners)
                dropdown.onValueChanged.AddListener(listener);
        }
    }
}