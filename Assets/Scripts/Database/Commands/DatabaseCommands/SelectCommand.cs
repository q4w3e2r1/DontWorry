using Scripts.Extensions;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SelectCommand : DatabaseCommand
{
    [SerializeField] private TMP_Dropdown _tableName;
    [SerializeField] private TMP_Dropdown _selectValue;

    private void Start()
    {
        var columns = _dbManager.ConnectedDatabase.Tables.Keys.ToArray();
        _tableName.SetOptions(columns);
        _tableName.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));

        var selectedValueOptions = new string[] { "*" };
        _selectValue.SetOptions(selectedValueOptions);
        _selectValue.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));
    }

    public void UpdateSelectedValue()
    {
        if (_tableName.captionText.text == "...")
            return;

        var columns = _dbManager.ConnectedDatabase.Tables[_tableName.captionText.text].ColumsDictionary.Keys.ToArray();
        var selectedValueOptions = new List<string>();
        selectedValueOptions.Add("*");
        foreach (var column in columns)
            selectedValueOptions.Add(column);
        _selectValue.SetOptions(selectedValueOptions.ToArray());
    }

    public override bool Execute()
    {
        var tableName = _tableName.captionText.text;
        var selectValue = _selectValue.captionText.text;

        if (tableName == "..." || selectValue == "...")
            return false;

        SaveBackup();

        Write(_dbManager.Select(tableName, selectValue));
        return true;
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}
