using SQL_Quest.Extentions;
using TMPro;
using UnityEngine;

namespace SQL_Quest.Database.Commands
{
    public class CreateDatabaseCommand : DatabaseCommand
    {
        [SerializeField] private TMP_Dropdown _name;

        private void Start()
        {
            _name.SetOptions(_dbManager.AllowedDatabases);
            _name.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));
        }

        public override bool Execute()
        {
            var name = _name.captionText.text;
            if (name == "...")
                return false;

            SaveBackup();
            _dbManager.CreateDatabase(name);
            Write("Query OK, 1 row affected");

            return true;
        }

        public override void Undo()
        {
            _dbManager.DropDatabase(_name.captionText.text);

            _output.text = _backup;
            Destroy(gameObject);
        }
    }
}