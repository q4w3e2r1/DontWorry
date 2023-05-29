using SQL_Quest.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Components.UI.ThemeController
{
    public class ThemeController : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [Space] [Header("Sprite")]
        [SerializeField] private Sprite _darkThemeSprite;
        [SerializeField] private Sprite _lightThemeSprite;
        [Space] [Header("Color")]
        [SerializeField] private Color _darkThemeColor;
        [SerializeField] private Color _lightThemeColor;

        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();

            switch (_mode)
            {
                case Mode.Sprite:
                    ChangeSprite();
                    break;
                case Mode.Color:
                    ChangeColor(); 
                    break;
                case Mode.Both:
                    ChangeSprite();
                    ChangeColor();
                    break;
            }
            
        }

        private void ChangeSprite()
        {
            if (!PlayerPrefs.HasKey("DarkTheme"))
            {
                _image.sprite = _lightThemeSprite;
                return;
            }
            if (PlayerPrefsExtensions.GetBool("DarkTheme"))
                _image.sprite = _lightThemeSprite;
            else
                _image.sprite = _darkThemeSprite;
        }

        private void ChangeColor() 
        {
            if (!PlayerPrefs.HasKey("DarkTheme"))
            {
                _image.color = _lightThemeColor;
                return;
            }
            if (PlayerPrefsExtensions.GetBool("DarkTheme"))
                _image.color = _lightThemeColor;
            else
                _image.color = _darkThemeColor;
        }
    }

    public enum Mode
    { 
        Sprite,
        Color,
        Both
    }
}
