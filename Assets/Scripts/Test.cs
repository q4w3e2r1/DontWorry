using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Test : MonoBehaviour
{
    private IDbConnection _connection;

    public void ExecuteQueryWithoutAnswer(string query)
    {
        IDbCommand dbCommand = _connection.CreateCommand();
        dbCommand.CommandText = query;
        dbCommand.ExecuteNonQuery();
    }

    private void Start()
    {
        var values = new string[]
        {
            "Агата",
            "Джек",
            "Джулиана",
            "Дэвид",
            "Нэнси",
            "Оливер",
            "Ванесса",
            "Эдгар"
        };

        _connection = new SqliteConnection($"URI=file:{Application.dataPath}/work-2023.sqlite");
        _connection.Open();

        ExecuteQueryWithoutAnswer("CREATE TABLE employee (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL)");
        foreach(var value in values)
            ExecuteQueryWithoutAnswer($"INSERT INTO employee (name) VALUES (\"{value}\")");
    }
}
