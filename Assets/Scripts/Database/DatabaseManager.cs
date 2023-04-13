using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TMPro;
using UnityEngine;
using System;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField] private string _databasesFolder;
    [SerializeField] private TextMeshProUGUI _output;
    [Space]
    [SerializeField] private string[] _databasesName;
    [SerializeField] private List<Table> _tables = new();
    
    private Dictionary<string, Dictionary<string, Table>> _allowedDatabases = new();
    private Dictionary<string, Database> _existingDatabases = new();

    public string[] AllowedColumnTypes;
    [HideInInspector] public string[] AllowedDatabases => _allowedDatabases.Keys.ToArray();
    [HideInInspector] public string[] ExistingDatabases => _existingDatabases.Keys.ToArray();

    private Database _connectedDatabaseName;

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
    }

    public void DropDatabase(string name)
    {
        if (_connectedDatabaseName.Name == name)
        {
            _existingDatabases[name].Disconnect();
            _connectedDatabaseName = null;
        }

        _existingDatabases.Remove(name);
    }

    public void UseDatabase(string name)
    { 
        _connectedDatabaseName?.Disconnect();
        _connectedDatabaseName = _existingDatabases[name];
        _connectedDatabaseName.Connect();
    }

    public string ShowDatabases()
    {
        return Table.Write("Databases", ExistingDatabases);
    }

    #endregion

    #region Table

    public void CreateTable(string name, string[] columnNames, string[] columnTypes) 
    {
        if (_connectedDatabaseName.Tables.Contains(name))
            DropTable(name);

        var columns = new Dictionary<string, string>();
        var columnsText = new string[columnNames.Length];
        for (int i = 0; i < columnNames.Length; i++)
        {
            columns[columnNames[i]] = columnTypes[i];
            columnsText[i] = $"{columnNames[i]} {columnTypes[i]}";
        }
        
        _connectedDatabaseName.ExecuteQueryWithoutAnswer($"CREATE TABLE {name}({string.Join(", ", columnsText)})");
        _connectedDatabaseName.CreateTable(name, columns);
    }

    public void DropTable(string name)
    {
        _connectedDatabaseName.ExecuteQueryWithoutAnswer($"DROP TABLE {name}");
        _connectedDatabaseName.DropTable(name);
    }

    public string[] GetTables()
        => _connectedDatabaseName.Tables.ToArray();

    public string[] GetTablesColums(string name)
        => _connectedDatabaseName.ShowTableColumns(name);

    public string ShowTables()
        => Table.Write($"Tables_in_{_connectedDatabaseName}", _connectedDatabaseName.Tables.ToArray());

    #endregion

    #region CommandPattern
    private Stack<Command> _commandHistory = new();
    private Stack<Command> _cancelledCommands = new();

    public void ExecuteCommand(Command command)
    {
        _cancelledCommands.Clear();

        if (command.Execute())
            _commandHistory.Push(command);
    }

    public void Undo()
    {
        _commandHistory.TryPop(out Command command);
        Debug.Log(command.GetType());
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
        Debug.Log(message);
    }
    //=> _output.text += message + '\n';
}
