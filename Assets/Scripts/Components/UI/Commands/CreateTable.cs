using SQL_Quest.Components.UI;
using SQL_Quest.Components.UI.Line;
using SQL_Quest.Components.UI.Shell;
using SQL_Quest.Extentions;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SQL_Quest.UI.Commands
{
    public class CreateTable : UICommand
    {
        private TMP_InputField _tableName;

        protected override void Start()
        {
            base.Start();
            var startLine = GetComponentsInChildren<Line>()[0].gameObject;
            _tableName = startLine.GetComponentsInChildren<TMP_InputField>()[0];
            SetLine();
        }

        public void SetLine()
        {
            var line = GetComponentsInChildren<Line>()[^1].gameObject;

            var columnTypesDropdown = line.GetComponentInChildren<TMP_Dropdown>();
            columnTypesDropdown.SetOptions(_dbManager.AllowedColumnTypes);
            columnTypesDropdown.onValueChanged.AddListener(value => Execute());

            var tableNameInputField = line.GetComponentsInChildren<TMP_InputField>()[^1];
            tableNameInputField.onEndEdit.AddListener(value => Execute());

            var modifier = line.GetComponentInChildren<Modifier>();
            modifier.AddListeners(() => DestoyColumnAttributeDropdown(line), () => CreateColumnAttributeDropdown(line));
        }

        private void CreateColumnAttributeDropdown(GameObject line)
        {
            var dropdownPrefab = Resources.Load<GameObject>("UI/Commands/Dropdowns/CreateDropdown");
            var columnTypesDropdown = Instantiate(dropdownPrefab, line.transform).GetComponent<TMP_Dropdown>();
            columnTypesDropdown.SetOptions(_dbManager.AllowedColumnAttributes);
            columnTypesDropdown.onValueChanged.AddListener(value => Execute());

            var textPrefab = Resources.Load<GameObject>("UI/Text");
            var separator = Instantiate(textPrefab, line.transform).GetComponent<TextMeshProUGUI>();
            separator.text = ",";

            var modifier = line.GetComponentInChildren<Modifier>();

            SetAsLastSiblings(modifier.gameObject, separator.gameObject);
        }

        private static void SetAsLastSiblings(params GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
                gameObject.transform.SetAsLastSibling();
        }

        private void DestoyColumnAttributeDropdown(GameObject line)
        {
            var dropdowns = line.GetComponentsInChildren<TMP_Dropdown>();
            if (dropdowns.Length < 2)
                return;
            Destroy(dropdowns[^1].gameObject);
        }

        public void Execute()
        {
            if (_tableName.text == "")
                return;

            var lines = GetComponentsInChildren<Line>();

            var columnNames = new string[lines.Length];
            var columnTypes = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var columnName = lines[i].GetComponentInChildren<TMP_InputField>().text;
                var columnType = lines[i].GetComponentsInChildren<TMP_Dropdown>()
                                         .Select(dropdown => dropdown.Text())
                                         .ToHashSet();
                if (columnName == "" || columnType.Contains("..."))
                    return;

                columnNames[i] = columnName;
                columnTypes[i] = string.Join(" ", columnType);
            }
            _dbManager.CreateTable(gameObject, _tableName.text, columnNames, columnTypes);
        }
    }
}