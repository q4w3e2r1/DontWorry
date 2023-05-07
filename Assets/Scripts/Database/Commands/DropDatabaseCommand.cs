namespace SQL_Quest.Database.Commands
{
    public class DropDatabaseCommand : DatabaseCommand
    {
        private string _name;

        public void Constructor(string name, bool returnMessage = true)
        {
            _name = name;
            Constructor(CommandType.Simple, returnMessage);
        }

        public override bool Execute()
        {
            if (_dbManager.ConnectedDatabase?.Name == _name)
            {
                _dbManager.ExistingDatabases[_name].Disconnect();
                _dbManager.ConnectedDatabase = null;
            }
            _dbManager.ExistingDatabases.Remove(_name);

            if (!_returnMessage)
                return false;

            SaveBackup();
            _chat.CheckMessage($"DROP DATABASE {_name}");
            Write("Query OK, 0 row affected");
            return true;
        }

        public override void Undo()
        {
            var undoCommand = gameObject.AddComponent<CreateDatabaseCommand>();
            undoCommand.Constructor(_name, false);
            undoCommand.Execute();
            base.Undo();
        }
    }
}