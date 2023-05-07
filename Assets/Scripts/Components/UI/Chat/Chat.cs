using SQL_Quest.Extentions;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Components.UI.Chat
{
    public class Chat : MonoBehaviour
    {
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _completeButton;
        [SerializeField] private Button _restartButton;
        [Space]
        [SerializeField] private GameObject _messagePrefab;
        [Space]
        [SerializeField] private Message[] _messages;
        [SerializeField] private Message[] _helpMessages;
        [SerializeField] private Message[] _errorMessage;

        private Stack<GameObject> _sentMessages = new();

        [HideInInspector] public int SentMessagesCount => _sentMessages.Count;

        private void Start()
        {
            SendMessage(_messages[0]);
        }

        public void CheckMessage(string message)
        {
            Debug.Log(message);
            var firstMessage = _messages.Where(msg => !msg.IsSent).First();
            var errorMessage = _errorMessage.Where(msg => msg.ConditionsForSending == message).FirstOrDefault();
            if (errorMessage != null)
            {
                SendMessage(errorMessage);
                return;
            }
            if (firstMessage.ConditionsForSending != message)
                return;

            SendMessage(firstMessage);
        }

        public void SendHelpMessage()
        {
            SendMessage(_helpMessages[0]);
        }

        private void SendMessage(Message message)
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