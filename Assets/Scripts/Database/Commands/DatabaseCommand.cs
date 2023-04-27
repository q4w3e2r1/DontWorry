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
        protected string _backup;
        private void Awake()
        {
            _dbManager = GameObject.FindWithTag("DatabaseManager").GetComponent<DatabaseManager>();
            _output = GameObject.FindWithTag("Output").GetComponent<TextMeshProUGUI>();
        }

        protected void SaveBackup()
        {
            _backup = _output.text;
        }

        public void Write(string message)
        {
            _output.text += message + '\n';
            _output.GetComponentInParent<ScrollRect>().ScrollToBottom();
        }

        public abstract void Undo();

        public abstract bool Execute();
    }
}