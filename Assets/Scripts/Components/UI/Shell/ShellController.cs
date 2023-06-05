using SQL_Quest.Creatures.Player;
using TMPro;
using UnityEngine;

namespace SQL_Quest.Components.UI.Shell
{
    public class ShellController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private TextMeshProUGUI _output;
        [Header("Commands")]
        [SerializeField] private GameObject _createDatabase;
        [SerializeField] private GameObject _showDatabases;
        [SerializeField] private GameObject _useDatabases;
        [SerializeField] private GameObject _dropDatabases;
        [SerializeField] private GameObject _createTable;
        [SerializeField] private GameObject _showTables;
        [SerializeField] private GameObject _selectTable;
        [SerializeField] private GameObject _insertInto;
        [SerializeField] private GameObject _update;
        [SerializeField] private GameObject _dropTable;

        private void Start()
        {
            var levelNumber = PlayerPrefs.GetInt("LevelNumber");

            var shellData = Resources.Load<ShellData>($"Levels/Level{levelNumber}/Shell/ShellData");
            shellData.Initialize();
            _levelName.text = shellData.LevelName;
            _output.text = "Welcome to the MySQL monitor.  Commands end with ; or \\g.\r\n" +
                $"Your MySQL connection id is {levelNumber}\r\n" +
                "Server version: 8.0.32 MySQL Community Server - GPL\r\n" +
                "\r\n" +
                "Copyright (c) 2000, 2023, Oracle and/or its affiliates.\r\n" +
                "\r\n" +
                "Oracle is a registered trademark of Oracle Corporation and/or its\r\n" +
                "affiliates. Other names may be trademarks of their respective\r\nowners.\n\n";

            var commands = new GameObject[]
            {
                _createDatabase, _showDatabases, _useDatabases, _dropDatabases,
                _createTable, _showTables, _selectTable, _insertInto, _insertInto, _update, _dropTable
            };
            for (int i = 0; i < commands.Length; i++)
                commands[i].SetActive(shellData.Command[i]);
        }
    }
}


