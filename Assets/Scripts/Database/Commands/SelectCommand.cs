using SQL_Quest.Extentions;
using System.Linq;

namespace SQL_Quest.Database.Commands
{
    public class SelectCommand : DatabaseCommand
    {
        private string _tableName;
        private string _selectedValue;
        private string _filter;
        private bool _writeManyColumns;

        public void Constructor(string tableName, string selectedValue, string filter, bool writeManyColumns, bool returnMessage = true)
        {
            _tableName = tableName;
            _selectedValue = selectedValue;
            _filter = filter;
            _writeManyColumns = writeManyColumns;
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

            var command = $"SELECT {_selectedValue} FROM {_tableName}{_filter}";
            UnityEngine.Debug.Log(command);

            if (!_returnMessage)
                return false;

            if (_writeManyColumns)
            {
                var reader = _dbManager.ConnectedDatabase.ExecuteQueryWithReader(command);

                var header = _dbManager.ConnectedDatabase.Tables[_tableName].ColumnsNames;
                var rows = reader.GetRows();
                if (!header.Contains(_selectedValue))
                    Write(Table.Write(header, rows));
                else
                {
                    var row = rows.Select(row => row[0]).ToArray();
                    Write(Table.Write(_selectedValue, row));
                }
            }
            else
            {
                var header = _selectedValue;
                var row = new string[] { _dbManager.ConnectedDatabase.ExecuteQueryWithAnswer(command) };
                Write(Table.Write(header, row));
            }

            _chat.CheckMessage(command);
            return true;
        }
    }
}