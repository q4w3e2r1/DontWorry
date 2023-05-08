namespace SQL_Quest.Database.Commands
{
    public class DropDatabaseCommand : DatabaseCommand
    {
        private string _name;
        private bool _isDatabaseConnected;

        public void Constructor(string name, bool returnMessage = true)
        {
            _name = name;
            Constructor(CommandType.Simple, returnMessage);
            _isDatabaseConnected = _dbManager.ConnectedDatabase?.Name == _name;
        }

        public override bool Execute()
        {
            base.Execute();
            SaveBackup();

            if (_isDatabaseConnected)
            {
                _dbManager.ExistingDatabases[_name].Disconnect();
                _dbManager.ConnectedDatabase = null;
            }
            _dbManager.ExistingDatabases.Remove(_name);

            if (!_returnMessage)
                return false;

            _chat.CheckMessage($"DROP DATABASE {_name}");
            Write("Query OK, 0 row affected");
            return true;
        }

        public override void Undo()
        {
            var undoCommand = gameObject.AddComponent<CreateDatabaseCommand>();
            undoCommand.Constructor(_name, false);
            undoCommand.Execute();
            Destroy(undoCommand);
            if (_isDatabaseConnected)
            {
                var useCommand = gameObject.AddComponent<UseDatabaseCommand>();
                useCommand.Constructor(_name, false);
                useCommand.Execute();
                Destroy(useCommand);
            }
            base.Undo();
        }
    }
}