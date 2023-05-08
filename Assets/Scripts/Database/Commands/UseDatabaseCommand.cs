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

        public override bool Execute()
        {
            base.Execute();
            SaveBackup();

            if (_name == "")
            {
                _dbManager.ConnectedDatabase = null;
            }
            else
            {
                _dbManager.ConnectedDatabase = _dbManager.ExistingDatabases[_name];
                _dbManager.ConnectedDatabase.Connect();
            }
            _databaseBackup = _dbManager.ConnectedDatabase;

            if (!_returnMessage)
                return false;

            _chat.CheckMessage($"USE {_name}");
            Write("Database changed");
            return true;
        }

        public override void Undo()
        {
            var undoCommand = gameObject.AddComponent<UseDatabaseCommand>();
            if (_databaseBackup != null)
                undoCommand.Constructor(_databaseBackup.Name, false);
            else
                undoCommand.Constructor("", false);
            undoCommand.Execute();
            Destroy(undoCommand);
            base.Undo();
        }
    }
}