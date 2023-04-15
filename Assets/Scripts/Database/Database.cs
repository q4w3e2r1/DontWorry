﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;

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

    public string Connect()
    {
        _connection.Open();
        return "Database changed";
    }

    public void Disconnect() 
        => _connection?.Close();

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
        => Tables[name] = new Table(name, colums);

    public void DropTable(string name)
        => Tables.Remove(name);

    public string[] ShowTableColumns(string name)
        => Tables[name].Columns;
}