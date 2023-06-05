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
    public class ThemeController : MonoBehaviour
    {
        [SerializeField] private GameObject _darkCanvas;
        [SerializeField] private GameObject _lightCanvas;

        private void Start()
        {
            if (!PlayerPrefs.HasKey("DarkTheme"))
                PlayerPrefsExtensions.SetBool("DarkTheme", false);

            var isDarkTheme = PlayerPrefsExtensions.GetBool("DarkTheme");
            _darkCanvas.SetActive(isDarkTheme);
            _lightCanvas.SetActive(!isDarkTheme);
        }
    }
}
