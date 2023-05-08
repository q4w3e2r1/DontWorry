using System.Linq;

namespace SQL_Quest.Database.Commands
{
    public class DropTableCommand : DatabaseCommand
    {
        private string _name;
        private Table _table;

        public void Constructor(string name, bool returnMessage = true)
        {
            _name = name;
            Constructor(CommandType.Simple, returnMessage);
        }

        public override bool Execute()
        {
            base.Execute();
            SaveBackup();

            if (_dbManager.ConnectedDatabase == null)
            {
                Write("ERROR 1046 (3D000): No database selected");
                return true;
            }

            var command = $"DROP TABLE {_name}";
            _dbManager.ConnectedDatabase.ExecuteQueryWithoutAnswer(command);
            _dbManager.ConnectedDatabase.DropTable(_name);

            if (!_returnMessage)
                return false;

            _chat.CheckMessage(command);
            Write("Query OK, 0 row affected");
            return true;
        }

        protected override void SaveBackup()
        {
            _table = _dbManager.ConnectedDatabase.Tables[_name];
            base.SaveBackup();
        }

        public override void Undo()
        {
            var undoCommand = gameObject.AddComponent<CreateTableCommand>();
            undoCommand.Constructor(_table.Name, _table.Columns, _table.ColumsDictionary.Values.ToArray(), false);
            undoCommand.Execute();
            Destroy(undoCommand);
            base.Undo();
        }
    }
}