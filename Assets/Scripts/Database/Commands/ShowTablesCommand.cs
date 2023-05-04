using System.Linq;

namespace SQL_Quest.Database.Commands
{
    public class ShowTablesCommand : DatabaseCommand
    {
        public ShowTablesCommand(bool returnMessage = true) : base(returnMessage)
        {
        }

        public override bool Execute()
        {
            Initialize();
            if (!_returnMessage)
                return false;

            if (_dbManager.ConnectedDatabase == null)
            {
                Write("ERROR 1046 (3D000): No database selected");
                return _returnMessage;
            }
            SaveBackup();
            Write(Table.Write($"Tables_in_{_dbManager.ConnectedDatabase.Name}", _dbManager.ExistingDatabases.Keys.ToArray()));
            _chat.CheckMessage("SHOW TABLES");
            return true;
        }
    }
}