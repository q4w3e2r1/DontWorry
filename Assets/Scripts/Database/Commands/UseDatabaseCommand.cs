namespace SQL_Quest.Database.Commands
{
    public class UseDatabaseCommand : DatabaseCommand
    {
        private string _name;
        private Database _databaseBackup;

        public UseDatabaseCommand(string name, bool returnMessage = true) : base(returnMessage)
        {
            _name = name;
        }

        public override bool Execute()
        {
            Initialize();
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

        protected override void SaveBackup()
        {
            _databaseBackup = _dbManager.ConnectedDatabase;
            base.SaveBackup();
        }

        public override void Undo()
        {
            if (_databaseBackup != null)
                new UseDatabaseCommand(_databaseBackup.Name, false);
            else
                new UseDatabaseCommand("", false);
            base.Undo();
        }

        public override string ToString()
        {
            return "Use Database Command";
        }
    }
}