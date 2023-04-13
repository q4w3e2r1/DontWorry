using Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropTableCommand : Command
{
    [SerializeField] private TMP_Dropdown _name;

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
        _dbManager.Write("Query OK, 0 row affected");

        return true;
    }
}
