using Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UseDatabaseCommand : Command
{
    [SerializeField] private TMP_Dropdown _name;

    private void Start()
    {
        _name.SetOptions(_dbManager.ExistingDatabases);
        _name.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));
    }

    public override bool Execute()
    {
        var name = _name.captionText.text;
        if (name == "...")
            return false;

        SaveBackup();

        _dbManager.UseDatabase(name);
        _dbManager.Write("Database changed");

        return true;
    }
}