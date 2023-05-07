using SQL_Quest.Components.UI.Line;
using SQL_Quest.Extentions;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.UI.Commands
{
    public class CreateTable : UICommand
    {
        private TMP_InputField _tableName;

        protected new void Start()
        {
            base.Start();
            var startLine = GetComponentsInChildren<Line>()[0].gameObject;
            _tableName = startLine.GetComponentsInChildren<TMP_InputField>()[0];
            SetLine();
        }

        public void SetLine()
        {
            var line = GetComponentsInChildren<Line>()[^1].gameObject;

            var dropdown = line.GetComponentInChildren<TMP_Dropdown>();
            dropdown.SetOptions(_dbManager.AllowedColumnTypes);
            dropdown.onValueChanged.AddListener(value => Execute());

            var inputField = line.GetComponentsInChildren<TMP_InputField>()[^1];
            inputField.onEndEdit.AddListener(value => Execute());

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
            dropdown.onValueChanged.AddListener(value => Execute());

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

        public void Execute()
        {
            if (_tableName.text == "...")
                return;

            var lines = GetComponentsInChildren<Line>();

            var columnNames = new string[lines.Length];
            var columnTypes = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                var columnName = lines[i].GetComponentInChildren<TMP_InputField>().text;
                var columnType = lines[i].GetComponentsInChildren<TMP_Dropdown>().Select(dropdown => dropdown.captionText.text).ToHashSet();
                if (columnName == "" || columnType.Contains("..."))
                    return;

                columnNames[i] = columnName;
                columnTypes[i] = string.Join(" ", columnType);
            }
            _dbManager.CreateTable(_tableName.text, columnNames, columnTypes);
        }
    }
}