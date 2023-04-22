using Scripts.Extensions;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTableCommand : DatabaseCommand
{
    private TMP_InputField _tableName;

    private void Start()
    {
        var startLine = GetComponentsInChildren<Line>()[0].gameObject;
        _tableName = startLine.GetComponentsInChildren<TMP_InputField>()[0];
        SetLine();
    }

    public void SetLine()
    {
        var line = GetComponentsInChildren<Line>()[^1].gameObject;

        var dropdown = line.GetComponentInChildren<TMP_Dropdown>();
        dropdown.SetOptions(_dbManager.AllowedColumnTypes);
        dropdown.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));

        var inputField = line.GetComponentsInChildren<TMP_InputField>()[^1];
        inputField.onEndEdit.AddListener(value => _dbManager.ExecuteCommand(this));

        var button = line.GetComponentsInChildren<Button>();
        button[0].onClick.AddListener(() => DestoyColumnAttributeDropdown(line));
        button[1].onClick.AddListener(() => CreateColumnAttributeDropdown(line));
    }

    private void CreateColumnAttributeDropdown(GameObject line)
    {
        var dropdownPrefab = line.GetComponentInChildren<TMP_Dropdown>().gameObject;

        var buttons = line.GetComponentsInChildren<Button>();
        var text = line.GetComponentsInChildren<TextMeshProUGUI>()[^1].gameObject;

        var dropdown = Instantiate(dropdownPrefab, line.transform).GetComponent<TMP_Dropdown>();
        dropdown.SetOptions(_dbManager.AllowedColumnAttributes);
        dropdown.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));

        foreach (var button in buttons)
            button.transform.SetAsLastSibling();
        text.transform.SetAsLastSibling();
    }

    private void DestoyColumnAttributeDropdown(GameObject line)
    {
        var dropdowns = line.GetComponentsInChildren<TMP_Dropdown>();
        if (dropdowns.Length < 2)
            return;
        Destroy(dropdowns[^1].gameObject);
    }

    public override bool Execute()
    {
        if (_dbManager.ConnectedDatabase == null)
        {
            Write("ERROR 1046 (3D000): No database selected");
            return true;
        }

        var lines = GetComponentsInChildren<Line>();

        var columnNames = new string[lines.Length];
        var columnTypes = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            var columnName = lines[i].GetComponentInChildren<TMP_InputField>().text;
            var columnType = lines[i].GetComponentsInChildren<TMP_Dropdown>().Select(dropdown => dropdown.captionText.text).ToHashSet();
            if (columnName == "" || columnType.Contains("..."))
                return false;

            columnNames[i] = columnName;
            columnTypes[i] = string.Join(" ", columnType);
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