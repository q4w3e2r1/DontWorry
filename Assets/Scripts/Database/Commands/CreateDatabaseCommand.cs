using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Extensions;
using Unity.VisualScripting;

public class CreateDatabaseCommand : Command
{
    [SerializeField] private TMP_Dropdown _name;

    private Database _databaseBackup;

    public CreateDatabaseCommand(TMP_Dropdown name)
    { 
        _name = name;
    }

    private void Start()
    {
        _name.SetOptions(_dbManager.AllowedDatabases);
        _name.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));
    }

    public override bool Execute()
    {
        var name = _name.captionText.text;
        if (name == "...")
            return false;

        SaveBackup();

        Write("Query OK, 1 row affected");

        return true;
    }

    public new void Undo()
    {
        _dbManager.DropDatabase(_name.captionText.text);
        base.Undo();
    }
}