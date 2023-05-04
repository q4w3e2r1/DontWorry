using SQL_Quest.Database;
using UnityEngine;

namespace SQL_Quest.UI.Commands
{
    public class UICommand : MonoBehaviour
    {
        protected DatabaseManager _dbManager;

        protected void Start()
        {
            _dbManager = GameObject.FindWithTag("DatabaseManager").GetComponent<DatabaseManager>();
        }
    }
}