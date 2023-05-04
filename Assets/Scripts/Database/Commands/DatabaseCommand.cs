using SQL_Quest.Components.UI.Chat;
using SQL_Quest.Extentions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Database.Commands
{
    public abstract class DatabaseCommand : MonoBehaviour
    {
        protected DatabaseManager _dbManager;
        protected TextMeshProUGUI _output;
        protected Chat _chat;
        protected int _chatBackup;
        protected string _outputBackup;
        protected bool _returnMessage;

        public DatabaseCommand(bool returnMessage = true)
        {
            _returnMessage = returnMessage;
        }

        protected void Initialize()
        {
            _dbManager = GameObject.FindWithTag("DatabaseManager").GetComponent<DatabaseManager>();
            _output = GameObject.FindWithTag("Output").GetComponent<TextMeshProUGUI>();
            _chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
        }

        protected void SaveBackup()
        {
            _outputBackup = _output.text;
            _chatBackup = _chat.GetComponentsInChildren<HorizontalLayoutGroup>().Length;
        }

        public void Write(string message)
        {
            _output.text += message + '\n';
            _output.GetComponentInParent<ScrollRect>().ScrollToBottom();
        }

        public void Undo()
        {
            if (!_returnMessage)
                Destroy(gameObject);

            _output.text = _outputBackup;
            var messages = _chat.GetComponentsInChildren<HorizontalLayoutGroup>();
            while (messages.Length > _chatBackup)
                Destroy(messages[^1]);
        }

        public abstract bool Execute();
    }
}