using System;
using UnityEngine;

namespace SQL_Quest.Components.UI.Shell.Chat
{
    [Serializable]
    public class Message
    {
        [SerializeField] private string _text;
        [SerializeField] private string[] _answerTo;
        [SerializeField] private bool _changeButtonToComplete;
        [SerializeField] private bool _changeButtonToRestart;

        public string Text => _text;
        public string[] AnswerTo => _answerTo;
        public bool ChangeButtonToComplete => _changeButtonToComplete;
        public bool ChangeButtonToRestart => _changeButtonToRestart;

        public Message(string text)
        {
            _text = text;
            _answerTo = Array.Empty<string>();
            _changeButtonToComplete = false;
            _changeButtonToRestart = false;
        }
    }
}