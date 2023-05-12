using SQL_Quest.Extentions;

namespace SQL_Quest.Database.Commands
{
    public class SelectCommand : DatabaseCommand
    {
        private string _tableName;
        private string _selectedValue;
        private bool _writeManyColumns;

        public void Constructor(string tableName, string selectedValue, bool writeManyColumns, bool returnMessage = true)
        {
            _tableName = tableName;
            _selectedValue = selectedValue;
            _writeManyColumns = writeManyColumns;
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

            var command = $"SELECT {_selectedValue} FROM {_tableName}";


            if (_writeManyColumns)
            {
                var reader = _dbManager.ConnectedDatabase.ExecuteQueryWithReader(command);

                var header = _dbManager.ConnectedDatabase.Tables[_tableName].Columns;
                var rows = reader.GetRows();
                Write(Table.Write(header, rows));
            }
            else
            {
                var header = _selectedValue;
                var row = new string[] { _dbManager.ConnectedDatabase.ExecuteQueryWithAnswer(command) };                
                Write(Table.Write(header, row));
            }

            if (!_returnMessage)
                return false;

            _chat.CheckMessage(command);
            return true;
        }
    }
}