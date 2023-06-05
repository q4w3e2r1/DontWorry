using SQL_Quest.Components.UI;
using SQL_Quest.Creatures.Player;
using SQL_Quest.Database.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SQL_Quest.Database.Manager
{
    public class DatabaseManager : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DatabaseManagerData _bound;
        [SerializeField] private DatabaseManagerDef _external;

        public string DatabasesFolder => _data.DatabaseFolder;
        public string[] AllowedColumnTypes => _data.AllowedColumnTypes;
        public string[] AllowedColumnAttributes => _data.AllowedColumnAttributes;

        [HideInInspector] public Database ConnectedDatabase { get; set; }
        [HideInInspector] public Dictionary<string, Dictionary<string, Table>> AllowedDatabases = new();
        [HideInInspector] public Dictionary<string, Database> ExistingDatabases = new();

        private Stack<DatabaseCommand> _commandHistory = new();
        private Stack<DatabaseCommand> _cancelledCommands = new();

        private DatabaseManagerData _data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    case Mode.Level:
                        var levelNumber = PlayerPrefs.GetInt("LevelNumber");
                        return Resources.Load<DatabaseManagerDef>($"Levels/Level{levelNumber}/Shell/DatabaseManager").Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Awake()
        {
            Cursor.visible = true;

            foreach(var database in _data.Databases) 
            {
                var tableDictionary = new Dictionary<string, Table>();
                foreach (var table in database.Tables)
                    tableDictionary[table.Name] = table;

                switch (database.Type)
                {
                    case DatabaseInspectorType.Allowed:
                        AllowedDatabases[database.Name] = tableDictionary;
                        break;
                    case DatabaseInspectorType.Existing:
                        var path = $"{Application.dataPath}/StreamingAssets/Databases/{DatabasesFolder}";
                        ExistingDatabases[database.Name] = new Database(database.Name, path, tableDictionary);
                        break;
                }
            }
        }

        #region Database

        public void CreateDatabaseCommand(GameObject gameObject, string name)
        {
            var command = gameObject.AddComponent<CreateDatabaseCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void DropDatabaseCommand(GameObject gameObject, string name)
        {
            var command = gameObject.AddComponent<DropDatabaseCommand>();
            command.Constructor(name, false);
            ExecuteCommand(command);
        }

        public void UseDatabaseCommand(GameObject gameObject, string name)
        {
            var command = gameObject.AddComponent<UseDatabaseCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void ShowDatabasesCommand(GameObject gameObject)
        {
            var command = gameObject.AddComponent<ShowDatabasesCommand>();
            command.Constructor();
            ExecuteCommand(command);
        }

        #endregion

        #region Table

        public void CreateTableCommand(GameObject gameObject, string name, string[] columnNames, string[] columnTypes)
        {
            var command = gameObject.AddComponent<CreateTableCommand>();
            command.Constructor(name, columnNames, columnTypes);
            ExecuteCommand(command);
        }

        public void DropTableCommand(GameObject gameObject, string name)
        {
            var command = gameObject.AddComponent<DropTableCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void InsertIntoCommand(GameObject gameObject, string tableName, string[] columns, string[] values)
        {
            var command = gameObject.AddComponent<InsertIntoCommand>();
            command.Constructor(tableName, columns, values);
            ExecuteCommand(command);
        }

        public void SelectCommand(GameObject gameObject, string tableName, string selectedValue,
                           string filter, bool writeManyColumns = true)
        {
            var command = gameObject.AddComponent<SelectCommand>();
            command.Constructor(tableName, selectedValue, filter, writeManyColumns);
            ExecuteCommand(command);
        }

        public void UpdateCommand(GameObject gameObject, string tableName, string setLine, string filter)
        {
            var command = gameObject.AddComponent<UpdateCommand>();
            command.Constructor(tableName, setLine, filter);
            ExecuteCommand(command);
        }

        public void ShowTablesCommand(GameObject gameObject)
        {
            var command = gameObject.AddComponent<ShowTablesCommand>();
            command.Constructor();
            ExecuteCommand(command);
        }

        public string[] GetTables()
        => ConnectedDatabase.Tables.Keys.ToArray();

        public string[] GetTablesColums(string name)
            => ConnectedDatabase.ShowTableColumns(name);

        #endregion

        #region CommandPattern

        public void ExecuteCommand(DatabaseCommand command)
        {
            _cancelledCommands.Clear();

            if (command.Execute())
                _commandHistory.Push(command);
        }

        public void Undo()
        {
            _commandHistory.TryPop(out DatabaseCommand command);
            if (command == null)
                return;
            _cancelledCommands.Push(command);
            command.Undo();
        }

        public void Redo()
        {
            _cancelledCommands.TryPop(out DatabaseCommand command);
            if (command == null)
                return;
            _commandHistory.Push(command);
            command.Execute();
        }

        public void DiscardChanges()
        {
            while (_commandHistory.Count != 0)
                Undo();
            Cursor.visible = false;
        }

        #endregion
    }
}