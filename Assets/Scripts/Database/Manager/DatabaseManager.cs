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
                        return Resources.Load<DatabaseManagerDef>($"Levels/Level{levelNumber}/DatabaseManager").Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }



        private void Awake()
        {
            Cursor.visible = true;
            GetAllowedDatabases();
            GetExistingDatabases();
        }

        private void GetAllowedDatabases()
        {
            for (int i = 0; i < _data.Tables.Length; i++)
            {
                var databaseName = _data.Tables[i].DatabaseName;
                if (!AllowedDatabases.ContainsKey(databaseName))
                    AllowedDatabases[databaseName] = new();
                AllowedDatabases[databaseName][_data.Tables[i].Name] = _data.Tables[i];
            }
        }

        private void GetExistingDatabases()
        {
            var databaseExtension = ".sqlite";
            var path = $"{Application.dataPath}/StreamingAssets/Databases/{DatabasesFolder}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }

            var files = Directory.GetFiles(path, $"*{databaseExtension}").Select(file =>
                file.Substring(path.Length + 1, file.Length - databaseExtension.Length - 1 - path.Length));
            foreach (var file in files)
            {
                if (!AllowedDatabases.ContainsKey(file))
                    continue;
                ExistingDatabases[file] = new Database(file, path, AllowedDatabases[file]);
            }
        }

        #region Database

        public void CreateDatabase(GameObject gameObject, string name)
        {
            var command = gameObject.AddComponent<CreateDatabaseCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void DropDatabase(GameObject gameObject, string name)
        {
            var command = gameObject.AddComponent<DropDatabaseCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void UseDatabase(GameObject gameObject, string name)
        {
            var command = gameObject.AddComponent<UseDatabaseCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void ShowDatabases(GameObject gameObject)
        {
            var command = gameObject.AddComponent<ShowDatabasesCommand>();
            command.Constructor();
            ExecuteCommand(command);
        }

        #endregion

        #region Table

        public void CreateTable(GameObject gameObject, string name, string[] columnNames, string[] columnTypes)
        {
            var command = gameObject.AddComponent<CreateTableCommand>();
            command.Constructor(name, columnNames, columnTypes);
            ExecuteCommand(command);
        }

        public void DropTable(GameObject gameObject, string name)
        {
            var command = gameObject.AddComponent<DropTableCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void InsertInto(GameObject gameObject, string tableName, string[] columns, string[] values)
        {
            var command = gameObject.AddComponent<InsertIntoCommand>();
            command.Constructor(tableName, columns, values);
            ExecuteCommand(command);
        }

        public void Select(GameObject gameObject, string tableName, string selectedValue,
                           string filter, bool writeManyColumns = true)
        {
            var command = gameObject.AddComponent<SelectCommand>();
            command.Constructor(tableName, selectedValue, filter, writeManyColumns);
            ExecuteCommand(command);
        }

        public void ShowTables(GameObject gameObject)
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