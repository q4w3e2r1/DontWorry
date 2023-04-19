using UnityEngine;

public class CleanDatabase : MonoBehaviour
{
    public void Clean()
    {
        var allowedColumns = new string[] { "Агата", "Джек", "Джулиана", "Дэвид", "Нэнси", "Оливер", "Ванесса", "Эдгар" };
        var dbManager = GameObject.FindWithTag("DatabaseManager").GetComponent<DatabaseManager>();
        dbManager.ConnectedDatabase?.ExecuteQueryWithoutAnswer($"DELETE FROM employee WHERE name != \"\"");
        foreach (var allowedColumn in allowedColumns)
            dbManager.ConnectedDatabase?.ExecuteQueryWithoutAnswer($"INSERT INTO employee (name) VALUES (\"{allowedColumn}\")");
    }
}
