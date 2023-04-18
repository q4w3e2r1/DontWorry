using UnityEngine;

public class CleanDatabase : MonoBehaviour
{
    public void Clean()
    {
        var dbManager = GameObject.FindWithTag("DatabaseManager").GetComponent<DatabaseManager>();
        dbManager.ConnectedDatabase.ExecuteQueryWithoutAnswer("DELETE FROM employee WHERE name = \"ี่๋๏่\"");
    }
}
