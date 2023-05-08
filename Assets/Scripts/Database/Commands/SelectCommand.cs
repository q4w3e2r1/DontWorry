using SQL_Quest.Extentions;

namespace SQL_Quest.Database.Commands
{
    public class SelectCommand : DatabaseCommand
    {
        private string _tableName;
        private string _selectedValue;

        public void Constructor(string tableName, string selectedValue, bool returnMessage = true)
        {
            _tableName = tableName;
            _selectedValue = selectedValue;
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

            var command = $"SELECT {_selectedValue} FROM {_tableName}";
            var reader = _dbManager.ConnectedDatabase.ExecuteQueryWithReader(command);

            var header = _dbManager.ConnectedDatabase.Tables[_tableName].Columns;
            var rows = reader.GetRows();

            if (!_returnMessage)
                return false;

            _chat.CheckMessage(command);
            Write(Table.Write(header, rows));
            return true;
        }
    }
}