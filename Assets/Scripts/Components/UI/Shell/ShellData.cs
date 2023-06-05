using System;
using UnityEngine;

namespace SQL_Quest.Components.UI.Shell
{
    [CreateAssetMenu]
    [Serializable]
    public class ShellData : ScriptableObject
    {
        [SerializeField] private string _levelName;
        [Space]
        [Header("Commands")]
        [SerializeField] private bool _createDatabase;
        [SerializeField] private bool _showDatabases;
        [SerializeField] private bool _useDatabases;
        [SerializeField] private bool _dropDatabases;
        [SerializeField] private bool _createTable;
        [SerializeField] private bool _showTables;
        [SerializeField] private bool _selectTable;
        [SerializeField] private bool _insertInto;
        [SerializeField] private bool _update;
        [SerializeField] private bool _dropTable;

        public string LevelName => _levelName;

        [HideInInspector] public bool[] Command;

        public void Initialize()
        {
            Command = new bool[]
            {
                _createDatabase, _showDatabases, _useDatabases, _dropDatabases,
                _createTable, _showTables, _selectTable, _insertInto, _insertInto, _update, _dropTable
            };
        }
    }
}
