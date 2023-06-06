using SQL_Quest.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Components.UI
{
    public class LevelWindow : MonoBehaviour
    {
        private void OnEnable()
        {
            if (PlayerPrefs.HasKey("CompleteGame") && PlayerPrefsExtensions.GetBool("CompleteGame"))
                return;

            var availableLevels = PlayerPrefs.GetInt("LevelNumber");
            var buttons = GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length - 1; i++)
                if (i + 1 > availableLevels)
                    buttons[i].interactable = false;
        }
    }
}
