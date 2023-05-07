using System.Collections.Generic;
using Unity.VisualScripting;

namespace SQL_Quest.Database.Commands
{
    public class CreateTableCommand : DatabaseCommand
    {
        private string _name;
        private string[] _columnNames;
        private string[] _columnTypes;

        public void Constructor(
            string name,
            string[] columnNames,
            string[] columnTypes,
            bool returnMessage = true)
        {
            _name = name;
            _columnNames = columnNames;
            _columnTypes = columnTypes;
            Constructor(CommandType.Simple, returnMessage);
        }

        public override bool Execute()
        {
            if (_dbManager.ConnectedDatabase == null)
            {
                Write("ERROR 1046 (3D000): No database selected");
                return true;
            }

            if (_dbManager.ConnectedDatabase.Tables.ContainsKey(_name))
            {
                var undoCommand = gameObject.AddComponent<DropTableCommand>();
                undoCommand.Constructor(_name, false);
                undoCommand.Execute();
            }
                

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

        public override void Undo()
        {
            var undoCommand = gameObject.AddComponent<DropDatabaseCommand>();
            undoCommand.Constructor(name, false);
            undoCommand.Execute();
            base.Undo();
        }
    }
}