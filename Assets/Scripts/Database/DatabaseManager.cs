using Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField] private string _databasesFolder;
    [SerializeField] private TextMeshProUGUI _output;
    [Space]
    [SerializeField] private string[] _databasesName;
    [SerializeField] private List<Table> _tables = new();
    [Space]
    [SerializeField] private Chat _chat;

    private Dictionary<string, Dictionary<string, Table>> _allowedDatabases = new();
    private Dictionary<string, Database> _existingDatabases = new();

    private Stack<Command> _commandHistory = new();
    private Stack<Command> _cancelledCommands = new();

    public string[] AllowedColumnTypes;
    public string[] AllowedColumnAttributes;
    [HideInInspector] public Database ConnectedDatabase;
    [HideInInspector] public string[] AllowedDatabases => _allowedDatabases.Keys.ToArray();
    [HideInInspector] public string[] ExistingDatabases => _existingDatabases.Keys.ToArray();

    private void Awake()
    {
        GetAllowedDatabases();
        GetExistingDatabases();
    }

    private void GetAllowedDatabases()
    {
        foreach (var databaseName in _databasesName)
            _allowedDatabases[databaseName] = new();

        for (int i = 0; i < _tables.Count; i++)
        {
            var databaseName = _tables[i].DatabaseName;
            if (!_allowedDatabases.ContainsKey(databaseName))
                _allowedDatabases[databaseName] = new();
            _allowedDatabases[databaseName][_tables[i].Name] = _tables[i];
        }
    }

    private void GetExistingDatabases()
    {
        var databaseExtension = ".sqlite";
        var path = $"{Application.dataPath}/Databases/{_databasesFolder}";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            return;
        }

        var files = Directory.GetFiles(path, $"*{databaseExtension}").Select(file =>
            file.Substring(path.Length + 1, file.Length - databaseExtension.Length - 1 - path.Length));
        foreach (var file in files)
        {
            if (!_allowedDatabases.ContainsKey(file))
                continue;
            _existingDatabases[file] = new Database(file, path, _allowedDatabases[file]);
        }
    }

    #region Database

    public void CreateDatabase(string name)
    {
        var database = new Database(name,
            $"{Application.dataPath}/Databases/{_databasesFolder}",
            new Dictionary<string, Table>());

        _existingDatabases[name] = database;
        _existingDatabases[name].Connect();
        _existingDatabases[name].Disconnect();

        _chat.CheckMessage($"CREATE DATABASE {name}");
    }

    public void DropDatabase(string name)
    {
        if (ConnectedDatabase?.Name == name)
        {
            _existingDatabases[name].Disconnect();
            ConnectedDatabase = null;
        }
        _existingDatabases.Remove(name);

        _chat.CheckMessage($"DROP DATABASE {name}");
    }

    public void UseDatabase(string name)
    {
        ConnectedDatabase?.Disconnect();

        if (name == "")
        {
            ConnectedDatabase = null;
            return;
        }

        ConnectedDatabase = _existingDatabases[name];
        ConnectedDatabase.Connect();

        _chat.CheckMessage($"USE {name}");
    }

    public string ShowDatabases()
    {
        _chat.CheckMessage("SHOW DATABASES");
        return Table.Write("Databases", ExistingDatabases);
    }

    #endregion

    #region Table

    public void CreateTable(string name, string[] columnNames, string[] columnTypes)
    {
        if (ConnectedDatabase.Tables.ContainsKey(name))
            DropTable(name);

        var columns = new Dictionary<string, string>();
        var columnsText = new string[columnNames.Length];
        for (int i = 0; i < columnNames.Length; i++)
        {
            columns[columnNames[i]] = columnTypes[i];
            columnsText[i] = $"{columnNames[i]} {columnTypes[i]}";
        }

        var command = $"CREATE TABLE {name}({string.Join(", ", columnsText)})";
        ConnectedDatabase.ExecuteQueryWithoutAnswer(command);
        ConnectedDatabase.CreateTable(name, columns);

        _chat.CheckMessage(command);
    }

    public void DropTable(string name)
    {
        var command = $"DROP TABLE {name}";
        ConnectedDatabase.ExecuteQueryWithoutAnswer(command);
        ConnectedDatabase.DropTable(name);
        _chat.CheckMessage(command);
    }

    public void InsertInto(string tableName, string[] columns, string[] values)
    {
        var command = $"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES (\"{string.Join("\", \"", values)}\")";
        ConnectedDatabase.ExecuteQueryWithoutAnswer(command);
        _chat.CheckMessage(command);
    }

    public string Select(string tableName, string selectedValue)
    {
        var command = $"SELECT {selectedValue} FROM {tableName}";
        var reader = ConnectedDatabase.ExecuteQueryWithReader(command);

        var header = ConnectedDatabase.Tables[tableName].Columns;
        var rows = reader.GetRows();

        _chat.CheckMessage(command);

        return Table.Write(header, rows);
    }

    public string[] GetTables()
    => ConnectedDatabase.Tables.Keys.ToArray();

    public string[] GetTablesColums(string name)
        => ConnectedDatabase.ShowTableColumns(name);

    public string ShowTables()
    {
        if (ConnectedDatabase == null)
            return "ERROR 1046 (3D000): No database selected";
        else
        {
            _chat.CheckMessage("SHOW TABLES");
            return Table.Write($"Tables_in_{ConnectedDatabase}", ConnectedDatabase.Tables.Keys.ToArray());
        }
    }


    #endregion

    #region CommandPattern

    public void ExecuteCommand(Command command)
    {
        _cancelledCommands.Clear();

        if (command.Execute())
            _commandHistory.Push(command);
    }

    public void Undo()
    {
        _commandHistory.TryPop(out Command command);
        if (command == null)
            return;
        _cancelledCommands.Push(command);
        command.Undo();
    }

    public void Redo()
    {
        _cancelledCommands.TryPop(out Command command);
        if (command == null)
            return;
        _commandHistory.Push(command);
        command.Execute();
    }

    #endregion

    public void Write(string message)
    {
        _output.text += message + '\n';
        // Debug.Log(message);
    }
}

[Serializable]
public class Message
{
    public string Text;
    public string ConditionsForSending;
    public bool IsSent;
    public UnityEvent OnSending;
}