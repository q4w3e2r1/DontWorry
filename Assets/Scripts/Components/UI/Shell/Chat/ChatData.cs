using System;
using UnityEngine;

namespace SQL_Quest.Components.UI.Shell.Chat
{
    [Serializable]
    public class ChatData
    {
        [SerializeField] private Message[] _messages;
        [SerializeField] private Message[] _helpMessages;
        [SerializeField] private Message[] _errorMessage;

        public Message[] Messages => _messages;
        public Message[] HelpMessages => _helpMessages;
        public Message[] ErrorMessages => _errorMessage;
    }
}
