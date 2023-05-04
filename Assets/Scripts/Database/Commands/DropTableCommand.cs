using System.Linq;

namespace SQL_Quest.Database.Commands
{
    public class DropTableCommand : DatabaseCommand
    {
        private string _name;
        private Table _table;

        public DropTableCommand(string name, bool returnMessage = true) : base(returnMessage)
        {
            _name = name;
        }

        public override bool Execute()
        {
            Initialize();
            if (_dbManager.ConnectedDatabase == null)
            {
                if (_returnMessage)
                    Write("ERROR 1046 (3D000): No database selected");
                return true;
            }

            SaveBackup();

            var command = $"DROP TABLE {_name}";
            _dbManager.ConnectedDatabase.ExecuteQueryWithoutAnswer(command);
            _dbManager.ConnectedDatabase.DropTable(_name);

            if (!_returnMessage)
                return false;

            _chat.CheckMessage(command);
            Write("Query OK, 0 row affected");
            return true;
        }

        private new void SaveBackup()
        {
            _table = _dbManager.ConnectedDatabase.Tables[_name];
            base.SaveBackup();
        }

        public new void Undo()
        {
            new CreateTableCommand(_table.Name, _table.Columns, _table.ColumsDictionary.Values.ToArray(), false);
            base.Undo();
        }
    }
}