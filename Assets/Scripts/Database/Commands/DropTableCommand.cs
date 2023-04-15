using Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DropTableCommand : Command
{
    [SerializeField] private TMP_Dropdown _name;

    private Table _table;

    private void Start()
    {
        _name.SetOptions(_dbManager.GetTables());
        _name.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));
    }

    public override bool Execute()
    {
        var name = _name.captionText.text;
        if (name == "...")
            return false;

        SaveBackup();

        _dbManager.DropTable(name);
        Write("Query OK, 0 row affected");

        return true;
    }

    private new void SaveBackup()
    {
        _table = _dbManager.ConnectedDatabase.Tables[_name.captionText.text];
        base.SaveBackup();
    }

    public override void Undo()
    { 
        _dbManager.CreateTable(_table.Name, _table.Columns, _table.ColumsDictionary.Values.ToArray());

        _output.text = _backup;
        Destroy(gameObject);
    }
}