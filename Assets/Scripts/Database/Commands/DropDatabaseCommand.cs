namespace SQL_Quest.Database.Commands
{
    public class DropDatabaseCommand : DatabaseCommand
    {
        private string _name;

        public DropDatabaseCommand(string name, bool returnMessage = true) : base(returnMessage)
        {
            _name = name;
        }

        public override bool Execute()
        {
            Initialize();
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

        public new void Undo()
        {
            new CreateDatabaseCommand(_name, false).Execute();
            base.Undo();
        }
    }
}