using UnityEngine;

namespace SQL_Quest.Database.Commands
{
    public class DeleteCommand : DatabaseCommand
    {
        private string _tableName;
        private string[] _columns;
        private string[] _values;

        public void Constructor(string tableName, string[] columns, string[] values, bool returnMessage = true)
        {
            _tableName = tableName;
            _columns = columns;
            _values = values;
            Constructor(returnMessage);
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

            var subcommands = new string[_columns.Length];
            for (int i = 0; i < subcommands.Length; i++)
                subcommands[i] = $"{_columns[i]} == \"{_values[i]}\"";
            var command = $"DELETE FROM {_tableName} WHERE {string.Join(" AND ", subcommands)}";
            Debug.Log(command);
            _dbManager.ConnectedDatabase.ExecuteQueryWithoutAnswer(command);

            if (!_returnMessage)
                return false;

            _chat.CheckMessage(command);
            Write("Query OK, 1 row affected");
            return true;
        }

        public override void Undo()
        {
            var undoCommand = gameObject.AddComponent<InsertIntoCommand>();
            undoCommand.Constructor(_tableName, _columns, _values, false);
            undoCommand.Execute();
            Destroy(undoCommand);
            base.Undo();
        }
    }
}