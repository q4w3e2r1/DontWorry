using SQL_Quest.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Components.UI.Shell.Chat
{
    public class ChatComponent : MonoBehaviour
    {
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _completeButton;
        [SerializeField] private Button _restartButton;
        [Space]
        [SerializeField] private GameObject _messagePrefab;
        [Space]
        [SerializeField] private Mode _mode;
        [SerializeField] private ChatData _bound;
        [SerializeField] private ChatDef _external;

        private Button _activeButton;
        private Stack<GameObject> _sentMessages = new();

        private ChatData _data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    case Mode.Level:
                        var levelNumber = PlayerPrefs.GetInt("LevelNumber");
                        return Resources.Load<ChatDef>($"Levels/Level{levelNumber}/Shell/Chat").Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [HideInInspector] public int SentMessagesCount => _sentMessages.Count;

        private void Start()
        {
            _activeButton = _helpButton;
            SendMessage(_data.Messages[0], true);
        }

        public void CheckMessage(string message)
        {
            Debug.Log(message);
            var firstMessage = _data.Messages[_sentMessages.Count];
            var errorMessage = _data.ErrorMessages.Where(msg => msg.AnswerTo.Contains(message)).FirstOrDefault();
            if (errorMessage != null)
            {
                SendMessage(errorMessage, true);
                return;
            }

            if (firstMessage.AnswerTo.Contains(message))
                SendMessage(firstMessage, true);

            foreach (var answer in firstMessage.AnswerTo)
                if (answer[^1] == '*' && message.Contains(answer[..^2]))
                    SendMessage(firstMessage, true);
        }

        public void SendHelpMessage()
        {
            SendMessage(new Message(_data.Messages[_sentMessages.Count].AnswerTo[0]), false);
        }

        private void SendMessage(Message message, bool pushInStack)
        {
            var messageGO = Instantiate(_messagePrefab);
            messageGO.GetComponentInChildren<TextMeshProUGUI>().text = message.Text;
            messageGO.transform.SetParent(GetComponentInChildren<VerticalLayoutGroup>().transform);
            messageGO.transform.localScale = Vector3.one;

            if (message.ChangeButtonToComplete)
                ChangeButtonToCompleteButton();
            else if (message.ChangeButtonToRestart)
                ChangeButtonToRestartButton();

            messageGO.GetComponentInParent<ScrollRect>().ScrollToBottom(messageGO);

            if (pushInStack)
                _sentMessages.Push(messageGO);
        }

        public void DestroyLastMessage()
        {
            Destroy(_sentMessages.Pop());
        }

        public void ChangeButtonToHelpButton()
        {
            _activeButton.gameObject.SetActive(false);
            _helpButton.gameObject.SetActive(true);
            _activeButton = _helpButton;
        }

        public void ChangeButtonToCompleteButton()
        {
            _activeButton.gameObject.SetActive(false);
            _completeButton.gameObject.SetActive(true);
            _activeButton = _completeButton;
        }

        public void ChangeButtonToRestartButton()
        {
            _activeButton.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(true);
            _activeButton = _restartButton;
        }
    }
}