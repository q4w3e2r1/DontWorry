using SQL_Quest.Components.UI.Chat;
using SQL_Quest.Database.Manager;
using SQL_Quest.Extentions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Database.Commands
{
    public abstract class DatabaseCommand : MonoBehaviour
    {
        protected virtual DatabaseManager _dbManager { get; set; }
        protected virtual TextMeshProUGUI _output { get; set; }
        protected virtual ChatComponent _chat { get; set; }
        protected virtual int _chatBackup { get; set; }
        protected virtual string _outputBackup { get; set; }
        protected virtual bool _returnMessage { get; set; }
        public virtual CommandType Type { get; set; }

        public void Constructor(CommandType type = CommandType.Simple, bool returnMessage = true)
        {
            Type = type;
            _returnMessage = returnMessage;
            _dbManager = GameObject.FindWithTag("DatabaseManager").GetComponent<DatabaseManager>();
            _output = GameObject.FindWithTag("Output").GetComponent<TextMeshProUGUI>();
            _chat = GameObject.FindWithTag("Chat").GetComponent<ChatComponent>();
        }

        protected virtual void SaveBackup()
        {
            _outputBackup = _output.text;
            _chatBackup = _chat.SentMessagesCount;
        }

        public void Write(string message)
        {
            _output.text += message + '\n';
            _output.GetComponentInParent<ScrollRect>().ScrollToBottom();
        }

        public virtual void Undo()
        {
            _output.text = _outputBackup;
            while (_chat.SentMessagesCount > _chatBackup)
                _chat.DestroyLastMessage();

            if (_returnMessage)
                gameObject.SetActive(false);
        }

        public virtual bool Execute()
        {
            if (_returnMessage)
                gameObject.SetActive(true);
            return false;
        }
    }

    public enum CommandType
    {
        Simple,
        Complex
    }
}