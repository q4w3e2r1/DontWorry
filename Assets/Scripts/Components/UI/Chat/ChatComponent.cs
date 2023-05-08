using SQL_Quest.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Components.UI.Chat
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

        private Stack<GameObject> _sentMessages = new();

        private ChatData _data
        {
            get
            {
                return _mode switch
                {
                    Mode.Bound => _bound,
                    Mode.External => _external.Data,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
        }

        [HideInInspector] public int SentMessagesCount => _sentMessages.Count;

        private void Start()
        {
            SendMessage(_data.Messages[0]);
        }

        public void CheckMessage(string message)
        {
            Debug.Log(message);
            var firstMessage = _data.Messages.Where(msg => !msg.IsSent).First();
            var errorMessage = _data.ErrorMessages.Where(msg => msg.AnswerTo == message).FirstOrDefault();
            if (errorMessage != null)
            {
                SendMessage(errorMessage);
                return;
            }
            if (firstMessage.AnswerTo != message)
                return;

            SendMessage(firstMessage);
        }

        public void SendHelpMessage()
        {
            SendMessage(_data.HelpMessages[0]);
        }

        private void SendMessage(MessageData message)
        {
            var messageGO = Instantiate(_messagePrefab);
            messageGO.GetComponentInChildren<TextMeshProUGUI>().text = message.Text;
            messageGO.transform.SetParent(GetComponentInChildren<VerticalLayoutGroup>().transform);
            messageGO.transform.localScale = Vector3.one;

            message.OnSending?.Invoke();
            message.IsSent = true;

            messageGO.GetComponentInParent<ScrollRect>().ScrollToBottom(messageGO);

            _sentMessages.Push(messageGO);
        }

        public void DestroyLastMessage()
        {
            Destroy(_sentMessages.Pop());
        }

        public void ChangeButtonToCompleteButton()
        {
            _helpButton.gameObject.SetActive(false);
            _completeButton.gameObject.SetActive(true);
        }

        public void ChangeButtonToRestartButton()
        {
            _helpButton.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(true);
        }
    }
}