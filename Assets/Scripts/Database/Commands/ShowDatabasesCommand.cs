using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ShowDatabasesCommand : Command
{
    public ShowDatabasesCommand(ShowDatabasesCommand command) { }

    public ShowDatabasesCommand Copy(ShowDatabasesCommand command)
        => new ShowDatabasesCommand(command);

    private void Start()
    {
        _dbManager.ExecuteCommand(this);
    }

    public override bool Execute()
    {
        SaveBackup();

        Write(_dbManager.ShowDatabases());

        return true;
    }

    public override void Undo()
    {
        _output.text = _backup;
        Destroy(gameObject);
    }
}