using Scripts.Extensions;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsertIntoCommand : DatabaseCommand
{
    [SerializeField] private GameObject _textPrefab;
    private TMP_Dropdown _tableName;
    private GameObject _separator;
    private Line _columnsLine;
    private Line _valuesLine;

    private void Start()
    {
        if (_dbManager.ConnectedDatabase == null)
        {
            Write("ERROR 1046 (3D000): No database selected");
            return;
        }

        var firstLine = GetComponentsInChildren<Line>()[0];
        _tableName = firstLine.GetComponentInChildren<TMP_Dropdown>();
        _tableName.SetOptions(_dbManager.ConnectedDatabase.Tables.Keys.ToArray());
        _tableName.onValueChanged.AddListener(value => SetColumnTypesDropdowns());

        _separator = _textPrefab;
        _separator.GetComponent<TextMeshProUGUI>().text = ",";

        _columnsLine = GetComponentsInChildren<Line>()[1];
        SetColumnTypesDropdowns();

        _valuesLine = GetComponentsInChildren<Line>()[^1];
        _valuesLine.GetComponentInChildren<TMP_InputField>().onEndEdit.AddListener(value => _dbManager.ExecuteCommand(this));
    }

    public void SetColumnTypesDropdowns()
    {
        var dropdowns = _columnsLine.GetComponentsInChildren<TMP_Dropdown>();
        var dropdownOptions = _tableName.captionText.text == "..." ? new string[] { "NO TABLE SELECTED" } :
            _dbManager.ConnectedDatabase.Tables[_tableName.captionText.text].Columns;

        foreach (var dropdown in dropdowns)
        {
            dropdown.SetOptions(dropdownOptions);
            dropdown.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));
        }
    }

    public void CreateColumnTypeDropdown()
    {
        var dropdownPrefab = _columnsLine.GetComponentInChildren<TMP_Dropdown>().gameObject;
        var buttons = _columnsLine.GetComponentsInChildren<Button>();

        Instantiate(_separator, _columnsLine.transform);

        var dropdown = Instantiate(dropdownPrefab, _columnsLine.transform).GetComponent<TMP_Dropdown>();
        SetColumnTypesDropdowns();

        foreach (var button in buttons)
            button.transform.SetAsLastSibling();
        _columnsLine.GetComponentsInChildren<TextMeshProUGUI>()[^5].transform.SetAsLastSibling();

        Instantiate(_separator, _valuesLine.transform);
        var inputFieldPrefab = _valuesLine.GetComponentInChildren<TMP_InputField>().gameObject;
        var inputField = Instantiate(inputFieldPrefab, _valuesLine.transform).GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener(value => _dbManager.ExecuteCommand(this));
        _valuesLine.GetComponentsInChildren<TextMeshProUGUI>()[^4].transform.SetAsLastSibling();
    }

    public void DestoyColumnTypeDropdown()
    {
        var dropdowns = _columnsLine.GetComponentsInChildren<TMP_Dropdown>();
        if (dropdowns.Length < 2)
            return;
        Destroy(dropdowns[^1].gameObject);
        Destroy(_columnsLine.GetComponentsInChildren<TextMeshProUGUI>()[^5].gameObject);

        Destroy(_valuesLine.GetComponentsInChildren<TMP_InputField>()[^1].gameObject);
        Destroy(_valuesLine.GetComponentsInChildren<TextMeshProUGUI>()[^2].gameObject);
    }

    public override bool Execute()
    {
        if (_dbManager.ConnectedDatabase == null)
        {
            Write("ERROR 1046 (3D000): No database selected");
            return true;
        }

        var lines = GetComponentsInChildren<Line>();

        var tableName = lines[0].GetComponentInChildren<TMP_Dropdown>().captionText.text;
        var columns = _columnsLine.GetComponentsInChildren<TMP_Dropdown>().Select(dropdown => dropdown.captionText.text).ToArray();
        if (tableName == "..." || columns.Contains("..."))
            return false;

        var columsDuplicates = columns
            .GroupBy(column => column)
            .Where(column => column.Count() > 1)
            .Select(column => column.Key);

        foreach (var column in columsDuplicates)
        {
            Write($"ERROR 1110 (42000): Column '{column}' specified twice");
            return true;
        }

        var values = lines[^1].GetComponentsInChildren<TMP_InputField>().Select(inputField => inputField.text).ToArray();
        if (values.Contains(""))
            return false;

        SaveBackup();
        _dbManager.InsertInto(tableName, columns, values);
        return true;
    }

    public override void Undo()
    {
        _output.text = _backup;
        Destroy(gameObject);
    }
}
