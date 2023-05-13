using System.Linq;

namespace SQL_Quest.Database.Commands
{
    public class InsertIntoCommand : DatabaseCommand
    {
        private string _tableName;
        private string[] _columns;
        private string[] _values;

        public void Constructor(string tableName, string[] columns, string[] values, bool returnMessage = true)
        {
            _tableName = tableName;
            _columns = columns;
            _values = values;
            base.Constructor(CommandType.Simple, returnMessage);
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

            var columsDuplicates = _columns
                .GroupBy(column => column)
                .Where(column => column.Count() > 1)
                .Select(column => column.Key);
            foreach (var column in columsDuplicates)
            {
                Write($"ERROR 1110 (42000): Column '{column}' specified twice");
                return true;
            }

            var command = $"INSERT INTO {_tableName} ({string.Join(", ", _columns)}) VALUES (\"{string.Join("\", \"", _values)}\")";
            _dbManager.ConnectedDatabase.ExecuteQueryWithoutAnswer(command);

            if (!_returnMessage)
                return false;

            _chat.CheckMessage(command);
            Write("Query OK, 1 row affected");
            return true;
        }

        public override void Undo()
        {
            var undoCommand = gameObject.AddComponent<DeleteCommand>();
            undoCommand.Constructor(_tableName, _columns, _values, false);
            undoCommand.Execute();
            base.Undo();
        }
    }
}