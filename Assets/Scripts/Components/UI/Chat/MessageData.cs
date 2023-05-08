using System;
using UnityEngine;
using UnityEngine.Events;

namespace SQL_Quest.Components.UI.Chat
{
    [Serializable]
    public class MessageData
    {
        [SerializeField] private string _text;
        [SerializeField] private string _answerTo;
        [SerializeField] private UnityEvent _onSending;

        public string Text => _text;
        public string AnswerTo => _answerTo;

        public bool IsSent { get; set; }
        public UnityEvent OnSending => _onSending;
    }
}