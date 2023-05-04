using System.Collections.Generic;

namespace SQL_Quest.Database.Commands
{
    public class CreateTableCommand : DatabaseCommand
    {
        private string _name;
        private string[] _columnNames;
        private string[] _columnTypes;

        public CreateTableCommand(string name, string[] columnNames, string[] columnTypes, bool returnMessage = true) : base(returnMessage)
        {
            _name = name;
            _columnNames = columnNames;
            _columnTypes = columnTypes;
        }

        public override bool Execute()
        {
            Initialize();
            if (_dbManager.ConnectedDatabase == null)
            {
                Write("ERROR 1046 (3D000): No database selected");
                return true;
            }

            if (_dbManager.ConnectedDatabase.Tables.ContainsKey(_name))
                new DropTableCommand(_name, false).Execute();

            var columns = new Dictionary<string, string>();
            var columnsText = new string[_columnNames.Length];
            for (int i = 0; i < _columnNames.Length; i++)
            {
                columns[_columnNames[i]] = _columnTypes[i];
                columnsText[i] = $"{_columnNames[i]} {_columnTypes[i]}";
            }

            var command = $"CREATE TABLE {_name}({string.Join(", ", columnsText)})";
            _dbManager.ConnectedDatabase.CreateTable(_name, columns, command);

            if (!_returnMessage)
                return false;

            SaveBackup();
            _chat.CheckMessage(command);
            Write("Query OK, 0 row affected");
            return true;
        }

        public new void Undo()
        {
            new DropDatabaseCommand(_name, false).Execute();
            base.Undo();
        }
    }
}