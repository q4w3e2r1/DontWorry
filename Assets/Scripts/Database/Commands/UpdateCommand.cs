namespace SQL_Quest.Database.Commands
{
    public class UpdateCommand : DatabaseCommand
    {
        private string _tableName;
        private string _setValues;
        private string _filter;

        public void Constructor(string tableName, string selectedValue, string filter, bool returnMessage = true)
        {
            _tableName = tableName;
            _setValues = selectedValue;
            _filter = filter;
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

            var command = $"UPDATE {_tableName} SET {_setValues} WHERE {_filter}";
            _dbManager.ConnectedDatabase.ExecuteQueryWithoutAnswer(command);

            if (!_returnMessage)
                return false;

            Write("Query OK, 1 row affected");
            _chat.CheckMessage(command);
            return true;
        }
    }
}
