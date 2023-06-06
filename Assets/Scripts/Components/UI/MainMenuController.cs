using SQL_Quest.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Components.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _levelsButton;
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private GameObject _completeGameCanvas;

        private void Start()
        {
            Cursor.visible = true;
            _continueButton.interactable = PlayerPrefs.HasKey("Name");
            _levelsButton.interactable = (PlayerPrefs.HasKey("LevelNumber") && PlayerPrefs.GetInt("LevelNumber") > 1) ||
                                         (PlayerPrefs.HasKey("CompleteGame") && PlayerPrefsExtensions.GetBool("CompleteGame"));

            if (PlayerPrefs.HasKey("LevelNumber") && PlayerPrefs.GetInt("LevelNumber") > 5)
                CompleteGame();
        }

        private void CompleteGame()
        {
            _completeGameCanvas.SetActive(true);
            PlayerPrefs.DeleteKey("Name");
            PlayerPrefs.DeleteKey("LevelNumber");
            _continueButton.interactable = PlayerPrefs.HasKey("Name");
            PlayerPrefsExtensions.SetBool("CompleteGame", true);
        }

        public void StartNewGame()
        {
            PlayerPrefs.SetString("Name", _nameInputField.textComponent.text);
            PlayerPrefs.SetInt("LevelNumber", 1);
            Cursor.visible = false;
        }
    }
}
