using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;

public class Database
{
    public string Name;

    private IDbConnection _connection;

    private Dictionary<string, Table> _tables = new();

    public HashSet<string> Tables => _tables.Keys.ToHashSet();

    public Database(string name, string path, Dictionary<string, Table> tables)
    {
        Name = name;
        _connection = new SqliteConnection($"URI=file:{path}/{Name}.sqlite");
        _tables = tables;
    }

    public string Connect()
    {
        _connection.Open();
        return "Database changed";
    }

    public void Disconnect() 
        => _connection.Close();

    public void ExecuteQueryWithoutAnswer(string query)
    {
        IDbCommand dbCommand = _connection.CreateCommand();
        dbCommand.CommandText = query;
        dbCommand.ExecuteNonQuery();
    }

    public IDataReader ExecuteQueryWithReader(string query)
    {
        IDbCommand dbCommand = _connection.CreateCommand();
        dbCommand.CommandText = query;
        return dbCommand.ExecuteReader();
    }

    public void CreateTable(string name, Dictionary<string, string> colums) 
        => _tables[name] = new Table(name, colums);

    public void DropTable(string name)
        => _tables.Remove(name);

    public string[] ShowTableColumns(string name)
        => _tables[name].Columns;
}
