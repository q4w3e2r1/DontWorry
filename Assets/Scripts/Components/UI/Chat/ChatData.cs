using System;
using UnityEngine;

namespace SQL_Quest.Components.UI.Chat
{
    [Serializable]
    public class ChatData
    {
        [SerializeField] private MessageData[] _messages;
        [SerializeField] private MessageData[] _helpMessages;
        [SerializeField] private MessageData[] _errorMessage;

        public MessageData[] Messages => _messages;
        public MessageData[] HelpMessages => _helpMessages;
        public MessageData[] ErrorMessages => _errorMessage;
    }
}
