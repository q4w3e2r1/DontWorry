using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ShowTablesCommand : Command
{
    private void Start()
    {
        _dbManager.ExecuteCommand(this);
    }

    public override bool Execute()
    {
        SaveBackup();

        _dbManager.Write(_dbManager.ShowTables());

        return true;
    }
}