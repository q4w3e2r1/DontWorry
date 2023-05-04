using SQL_Quest.Extentions;

namespace SQL_Quest.Database.Commands
{
    public class SelectCommand : DatabaseCommand
    {
        private string _tableName;
        private string _selectedValue;

        public SelectCommand(string tableName, string selectedValue, bool returnMesaage = true) : base(returnMesaage)
        {
            _tableName = tableName;
            _selectedValue = selectedValue;
        }

        public override bool Execute()
        {
            Initialize();
            var command = $"SELECT {_selectedValue} FROM {_tableName}";
            var reader = _dbManager.ConnectedDatabase.ExecuteQueryWithReader(command);

            var header = _dbManager.ConnectedDatabase.Tables[_tableName].Columns;
            var rows = reader.GetRows();

            if (!_returnMessage)
                return false;

            SaveBackup();
            _chat.CheckMessage(command);
            Write(Table.Write(header, rows));
            return true;
        }
    }
}