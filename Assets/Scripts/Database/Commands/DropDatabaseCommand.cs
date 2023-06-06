namespace SQL_Quest.Database.Commands
{
    public class DropDatabaseCommand : DatabaseCommand
    {
        private string _name;
        private bool _deleteFile;
        private bool _isDatabaseConnected;

        public void Constructor(string name, bool deleteFile, bool returnMessage = true)
        {
            _name = name;
            _deleteFile = deleteFile;
            Constructor(returnMessage);
            _isDatabaseConnected = _dbManager.ConnectedDatabase?.Name == _name;
        }

        public override bool Execute()
        {
            base.Execute();
            SaveBackup();

            _dbManager.ExistingDatabases[_name].Disconnect();

            if (_deleteFile)
                _dbManager.ExistingDatabases[_name].Drop();

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