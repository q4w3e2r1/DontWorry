using SQL_Quest.Components.UI;
using SQL_Quest.Components.UI.Line;
using SQL_Quest.Components.UI.Shell;
using SQL_Quest.Extentions;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SQL_Quest.Components.UI.Commands
{
    public class CreateTable : UICommand
    {
        [SerializeField] private TMP_InputField _tableName;

        protected override void Start()
        {
            base.Start();
            _tableName.onEndEdit.AddListener(value => Execute());
            SetLine();
        }

        public void SetLine()
        {
            var line = GetComponentsInChildren<LineComponent>()[^1].gameObject;

            var columnTypesDropdown = line.GetComponentInChildren<TMP_Dropdown>();
            columnTypesDropdown.SetOptions(_dbManager.AllowedColumnTypes);
            columnTypesDropdown.onValueChanged.AddListener(value => Execute());

            var tableNameInputField = line.GetComponentInChildren<TMP_InputField>();
            tableNameInputField.onEndEdit.AddListener(value => Execute());

            var modifier = line.GetComponentInChildren<Modifier>();
            modifier.AddListeners(() => DestoyColumnAttributeDropdown(line), () => CreateColumnAttributeDropdown(line));
        }

        private void CreateColumnAttributeDropdown(GameObject line)
        {
            var dropdownPrefab = Resources.Load<GameObject>("UI/Shell/Dropdowns/CreateDropdown");

            var separator = line.GetComponentsInChildren<TextMeshProUGUI>()[^1];

            var columnAttributeDropdown = Instantiate(dropdownPrefab, line.transform).GetComponent<TMP_Dropdown>();
            columnAttributeDropdown.SetOptions(_dbManager.AllowedColumnAttributes);
            columnAttributeDropdown.onValueChanged.AddListener(value => Execute());

            line.GetComponentInChildren<Modifier>().transform.SetAsLastSibling();
            separator.transform.SetAsLastSibling();
        }

        private void DestoyColumnAttributeDropdown(GameObject line)
        {
            var dropdowns = line.GetComponentsInChildren<TMP_Dropdown>();
            if (dropdowns.Length == 1)
                return;

            Destroy(dropdowns[^1].gameObject);
        }

        public void Execute()
        {
            if (_tableName.text == "")
                return;

            var lines = GetComponentsInChildren<LineComponent>();

            var columnNames = new string[lines.Length];
            var columnTypes = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var columnName = lines[i].GetComponentInChildren<TMP_InputField>().text;
                var columnType = lines[i].GetComponentsInChildren<TMP_Dropdown>()
                                         .Select(dropdown => dropdown.GetText())
                                         .ToHashSet();
                if (columnName == "" || columnType.Contains("..."))
                    return;

                columnNames[i] = columnName;
                columnTypes[i] = string.Join(" ", columnType);
            }
            _dbManager.CreateTableCommand(gameObject, _tableName.text, columnNames, columnTypes);
        }
    }
}