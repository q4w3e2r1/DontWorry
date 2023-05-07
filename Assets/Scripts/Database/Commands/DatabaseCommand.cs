using SQL_Quest.Components.UI.Chat;
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
        protected virtual Chat _chat { get; set; }
        protected virtual int _chatBackup { get; set; }
        protected virtual string _outputBackup { get; set; }
        protected virtual bool _returnMessage { get; set; }

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

        protected virtual void SaveBackup()
        {
            _outputBackup = _output.text;
            _chatBackup = _chat.GetComponentsInChildren<HorizontalLayoutGroup>().Length;
        }

        public void Write(string message)
        {
            _output.text += message + '\n';
            _output.GetComponentInParent<ScrollRect>().ScrollToBottom();
        }

        public virtual void Undo()
        {
            if (!_returnMessage)
                Destroy(gameObject);

            _output.text = _outputBackup;
            var messages = _chat.GetComponentsInChildren<HorizontalLayoutGroup>();
            while (messages.Length > _chatBackup)
                Destroy(messages[^1]);
        }

        public abstract bool Execute();

        public override string ToString()
        {
            return "Database Command";
        }
    }
}