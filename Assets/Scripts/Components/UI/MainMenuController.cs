using SQL_Quest.Creatures.Player;
using TMPro;
using UnityEngine;

namespace SQL_Quest.Components.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _continueButton;
        [SerializeField] private TMP_InputField _nameInputField;

        private PlayerData _playerData;

        private void Start()
        {
            Cursor.visible = true;
            _playerData = PlayerDataHandler.PlayerData;
            _continueButton.SetActive(_playerData.Name != "");
        }

        public void StartNewGame()
        {
            _playerData.Name = _nameInputField.text;
            _playerData.LevelNumber = 1;
            Cursor.visible = false;
        }
    }
}
