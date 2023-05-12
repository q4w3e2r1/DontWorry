using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;

namespace SQL_Quest.Database
{
    public class Database
    {
        public string Name;

        private IDbConnection _connection;

        public Dictionary<string, Table> Tables = new();

        public Database(string name, string path, Dictionary<string, Table> tables)
        {
            Name = name;
            _connection = new SqliteConnection($"URI=file:{path}/{Name}.sqlite");
            Tables = tables;
        }

        public void Connect()
        {
            Disconnect();
            _connection.Open();
        }

        public void Disconnect()
            => _connection?.Close();

        public void ExecuteQueryWithoutAnswer(string query)
        {
            IDbCommand dbCommand = _connection.CreateCommand();
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }

        public string ExecuteQueryWithAnswer(string query)
        {
            IDbCommand dbCommand = _connection.CreateCommand();
            dbCommand.CommandText = query;
            var answer = dbCommand.ExecuteScalar();
            return answer.ToString();
        }

        public IDataReader ExecuteQueryWithReader(string query)
        {
            IDbCommand dbCommand = _connection.CreateCommand();
            dbCommand.CommandText = query;
            return dbCommand.ExecuteReader();
        }

        public void CreateTable(string name, Dictionary<string, string> colums, string command)
        {
            Tables[name] = new Table(name, colums);
            ExecuteQueryWithoutAnswer(command);
        }

        public void DropTable(string name)
            => Tables.Remove(name);

        public string[] ShowTableColumns(string name)
            => Tables[name].Columns;
    }
}