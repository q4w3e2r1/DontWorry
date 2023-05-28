using TMPro;
using UnityEngine;

namespace SQL_Quest.Components.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _continueButton;
        [SerializeField] private TMP_InputField _nameInputField;

        private void Start()
        {
            Cursor.visible = true;
            _continueButton.SetActive(PlayerPrefs.HasKey("Name"));
        }

        public void StartNewGame()
        {
            PlayerPrefs.SetString("Name", _nameInputField.textComponent.text);
            PlayerPrefs.SetInt("LevelNumber", 1);
            Cursor.visible = false;
        }
    }
}
