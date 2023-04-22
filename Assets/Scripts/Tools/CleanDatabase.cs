using UnityEngine;

public class CleanDatabase : MonoBehaviour
{
    public void Clean()
    {
        var allowedColumns = new string[] { "Агата", "Джек", "Джулиана", "Дэвид", "Нэнси", "Оливер", "Ванесса", "Эдгар" };
        var dbManager = GameObject.FindWithTag("DatabaseManager").GetComponent<DatabaseManager>();
        dbManager.ConnectedDatabase?.ExecuteQueryWithoutAnswer($"DROP TABLE employee");
        dbManager.ConnectedDatabase?.ExecuteQueryWithoutAnswer($"CREATE TABLE employee(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL)");
        foreach (var allowedColumn in allowedColumns)
            dbManager.ConnectedDatabase?.ExecuteQueryWithoutAnswer($"INSERT INTO employee (name) VALUES (\"{allowedColumn}\")");
    }
}
