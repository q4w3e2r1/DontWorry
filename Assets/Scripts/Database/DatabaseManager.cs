using SQL_Quest.Database.Commands;
using SQL_Quest.GoBased;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SQL_Quest.Database
{
    public class DatabaseManager : MonoBehaviour
    {
        [SerializeField] public string DatabasesFolder;
        [Space]
        [SerializeField] private string[] _databasesName;
        [SerializeField] private List<Table> _tables = new();

        [HideInInspector] public Dictionary<string, Dictionary<string, Table>> AllowedDatabases = new();
        [HideInInspector] public Dictionary<string, Database> ExistingDatabases = new();

        private Stack<DatabaseCommand> _commandHistory = new();
        private Stack<DatabaseCommand> _cancelledCommands = new();

        public string[] AllowedColumnTypes;
        public string[] AllowedColumnAttributes;
        [HideInInspector] public Database ConnectedDatabase;

        private void Awake()
        {
            GetAllowedDatabases();
            GetExistingDatabases();
        }

        private void GetAllowedDatabases()
        {
            foreach (var databaseName in _databasesName)
                AllowedDatabases[databaseName] = new();

            for (int i = 0; i < _tables.Count; i++)
            {
                var databaseName = _tables[i].DatabaseName;
                if (!AllowedDatabases.ContainsKey(databaseName))
                    AllowedDatabases[databaseName] = new();
                AllowedDatabases[databaseName][_tables[i].Name] = _tables[i];
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

        public void CreateDatabase(string name)
        {
            var command = gameObject.AddComponent<CreateDatabaseCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void DropDatabase(string name)
        {
            var command = gameObject.AddComponent<DropDatabaseCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void UseDatabase(string name)
        {
            var command = gameObject.AddComponent<UseDatabaseCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void ShowDatabases()
        {
            var command = gameObject.AddComponent<ShowDatabasesCommand>();
            command.Constructor();
            ExecuteCommand(command);
        }

        #endregion

        #region Table

        public void CreateTable(string name, string[] columnNames, string[] columnTypes)
        {
            var command = gameObject.AddComponent<CreateTableCommand>();
            command.Constructor(name, columnNames, columnTypes);
            ExecuteCommand(command);
        }

        public void DropTable(string name)
        {
            var command = gameObject.AddComponent<DropTableCommand>();
            command.Constructor(name);
            ExecuteCommand(command);
        }

        public void InsertInto(string tableName, string[] columns, string[] values)
        {
            var command = gameObject.AddComponent<InsertIntoCommand>();
            command.Constructor(tableName, columns, values);
            ExecuteCommand(command);
        }

        public void Select(string tableName, string selectedValue)
        {
            var command = gameObject.AddComponent<SelectCommand>();
            command.Constructor(tableName, selectedValue);
            ExecuteCommand(command);
        }

        public string[] GetTables()
        => ConnectedDatabase.Tables.Keys.ToArray();

        public string[] GetTablesColums(string name)
            => ConnectedDatabase.ShowTableColumns(name);

        public void ShowTables()
        {
            var command = gameObject.AddComponent<ShowTablesCommand>();
            command.Constructor();
            ExecuteCommand(command);
        }


        #endregion

        public void SpawnCommand(SpawnComponentInTarget spawner)
        {
            var command = gameObject.AddComponent<SpawnCommand>();
            command.Constructor(spawner);
            ExecuteCommand(command);
        }

        #region CommandPattern

        public void ExecuteCommand(DatabaseCommand command)
        {
            _cancelledCommands.Clear();

            if (command.Execute())
                _commandHistory.Push(command);
            Debug.Log($"Add {command} {_commandHistory.Count}");
        }

        public void Undo()
        {
            _commandHistory.TryPop(out DatabaseCommand command);
            if (command == null)
                return;
            _cancelledCommands.Push(command);
            command.Undo();
            if (command.Type == Commands.CommandType.Simple)
                Undo();
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
        }

        #endregion
    }
}