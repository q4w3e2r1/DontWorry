namespace SQL_Quest.Database.Commands
{
    public class UseDatabaseCommand : DatabaseCommand
    {
        private string _name;
        private Database _databaseBackup;

        public void Constructor(string name, bool returnMessage = true)
        {
            _name = name;
            Constructor(CommandType.Simple, returnMessage);
        }

        protected override void SaveBackup()
        {
            _databaseBackup = _dbManager.ConnectedDatabase;
            base.SaveBackup();
        }

        public override bool Execute()
        {
            _dbManager.ConnectedDatabase?.Disconnect();

            if (_name == "")
            {
                _dbManager.ConnectedDatabase = null;
            }
            else
            {
                _dbManager.ConnectedDatabase = _dbManager.ExistingDatabases[_name];
                _dbManager.ConnectedDatabase.Connect();
            }

            if (!_returnMessage)
                return false;

            SaveBackup();
            _chat.CheckMessage($"USE {_name}");
            Write("Database changed");
            return true;
        }

        public override void Undo()
        {
            if (_databaseBackup != null)
            {
                var undoCommand = gameObject.AddComponent<UseDatabaseCommand>();
                undoCommand.Constructor(_databaseBackup.Name, false);
                undoCommand.Execute();
            }
            else
            {
                var undoCommand = gameObject.AddComponent<UseDatabaseCommand>();
                undoCommand.Constructor("", false);
                undoCommand.Execute();
            }
            base.Undo();
        }
    }
}