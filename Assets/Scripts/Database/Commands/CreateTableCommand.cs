using Scripts.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTableCommand : Command
{
    [SerializeField] private GameObject _dropdownGO;
    [SerializeField] private GameObject _inputFieldGO;

    private TMP_Dropdown[] _columnTypes;

    public CreateTableCommand(GameObject dropdownGO, GameObject inputFieldGO)
    { 
        _dropdownGO = dropdownGO;
        _inputFieldGO = inputFieldGO;
    }

    private void Start()
    {        
        _columnTypes = GetComponentsInChildren<TMP_Dropdown>();
        foreach (var columnType in _columnTypes) 
        {
            columnType.SetOptions(_dbManager.AllowedColumnTypes);
        }

        _dropdownGO.GetComponent<TMP_Dropdown>().SetOptions(_dbManager.AllowedColumnTypes);
        _dropdownGO.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this)); ;
    }

    private void Update()
    {
        var last_dropdown = _columnTypes[^1];

        if (last_dropdown.captionText.text == "...")
            return;

        Instantiate(_inputFieldGO, this.transform);
        Instantiate(_dropdownGO, this.transform);

        _columnTypes = GetComponentsInChildren<TMP_Dropdown>();
    }

    public override bool Execute()
    {
        var columnInputFields = GetComponentsInChildren<TMP_InputField>();

        var name = columnInputFields[0].textComponent.text;
        if (name == "...")
            return false;
        
        var columnNames = columnInputFields.Select(x => x.textComponent.text).Where(x => x != "" && x != name).ToArray();
        var columnTypes = _columnTypes.Select(x => x.captionText.text).Where(x => x != "...").ToArray();

        if(columnNames.Length != columnTypes.Length)
            return false;

        SaveBackup();

        _dbManager.CreateTable(name, columnNames, columnTypes);
        _dbManager.Write("Query OK, 0 row affected");

        return true;
    }
}