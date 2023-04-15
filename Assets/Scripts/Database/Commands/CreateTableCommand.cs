using Scripts.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CreateTableCommand : Command
{
    [SerializeField] private GameObject _dropdownPrefab;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private TMP_Text _linesCount;

    private TMP_InputField _tableName;

    private void Start()
    {
        var startLine = GetComponentsInChildren<Line>()[0].gameObject;
        _tableName = startLine.GetComponentsInChildren<TMP_InputField>()[0];
        SetLine(GetComponentsInChildren<Line>()[^1].gameObject);
    }

    public void CheckLinesCount()
    {
        var linesCount = int.Parse(_linesCount.text);
        var lines = GetComponentsInChildren<Line>();
        var realLinesCount = lines.Length - 1;

        while (realLinesCount < linesCount)
        {
            CreateLine();
            realLinesCount++;
        }
        while (realLinesCount > linesCount)
        {
            DestroyLine();
            realLinesCount--;
        }
    }

    public void CreateLine()
    {
        var text = GetComponentsInChildren<TextMeshProUGUI>()[^1].gameObject;

        var line = Instantiate(_linePrefab, transform);
        SetLine(line);

        text.transform.SetAsLastSibling();
    }

    private void SetLine(GameObject line)
    {
        var dropdown = line.GetComponentInChildren<TMP_Dropdown>();
        dropdown.SetOptions(_dbManager.AllowedColumnTypes);
        dropdown.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));

        var inputField = line.GetComponentsInChildren<TMP_InputField>()[^1];
        inputField.onEndEdit.AddListener(value => _dbManager.ExecuteCommand(this));

        var button = line.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => CreateColumnAttributeDropdown(line));
    }

    private void DestroyLine()
    {
        Destroy(GetComponentsInChildren<Line>()[^1].gameObject);
    }

    public void CreateColumnAttributeDropdown(GameObject line)
    {
        var button = line.GetComponentInChildren<Button>();
        var text = line.GetComponentsInChildren<TextMeshProUGUI>()[^1].gameObject;

        var dropdown = Instantiate(_dropdownPrefab, line.transform).GetComponent<TMP_Dropdown>();
        dropdown.SetOptions(_dbManager.AllowedColumnAttributes);
        dropdown.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));

        button.transform.SetAsLastSibling();
        text.transform.SetAsLastSibling();
    }

    public override bool Execute()
    {
        var lines = GetComponentsInChildren<Line>();

        var columnNames = new string[lines.Length];
        var columnTypes = new string[lines.Length];

        for(int i = 0; i < lines.Length; i++)
        { 
            var columnName = lines[i].GetComponentInChildren<TMP_InputField>().text;
            var columnType = lines[i].GetComponentsInChildren<TMP_Dropdown>().Select(dropdown => dropdown.captionText.text).ToHashSet();
            if (columnName == "" || columnType.Contains("..."))
                return false;

            columnNames[i] = columnName;
            columnTypes[i] = string.Join(", ", columnType);
        }

        SaveBackup();

        _dbManager.CreateTable(_tableName.text, columnNames, columnTypes);
        Write("Query OK, 0 row affected");

        return true;
    }

    public override void Undo()
    {
        _dbManager.DropTable(GetComponentsInChildren<TMP_InputField>()[0].textComponent.text);

        _output.text = _backup;
        Destroy(gameObject);
    }
}