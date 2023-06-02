using SQL_Quest.Components.UI.Line;
using SQL_Quest.Extentions;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.UI.Commands
{
    public class InsertInto : UICommand
    {
        private TMP_Dropdown _tableName;
        private GameObject _separator;
        private Line _columnsLine;
        private Line _valuesLine;

        protected override void Start()
        {
            base.Start();
            if (_dbManager.ConnectedDatabase == null)
            {
                _dbManager.InsertInto(gameObject, "", Array.Empty<string>(), Array.Empty<string>());
                return;
            }

            var firstLine = GetComponentsInChildren<Line>()[0];
            _tableName = firstLine.GetComponentInChildren<TMP_Dropdown>();
            _tableName.SetOptions(_dbManager.ConnectedDatabase.Tables.Keys.ToArray());
            _tableName.onValueChanged.AddListener(value => SetColumnTypesDropdowns());

            _separator = Resources.Load<GameObject>("UI/Text");
            _separator.GetComponent<TextMeshProUGUI>().text = ",";

            _columnsLine = GetComponentsInChildren<Line>()[1];
            SetColumnTypesDropdowns();

            _valuesLine = GetComponentsInChildren<Line>()[^1];
            _valuesLine.GetComponentInChildren<TMP_InputField>().onEndEdit.AddListener(value => Execute());
        }

        public void SetColumnTypesDropdowns()
        {
            var dropdowns = _columnsLine.GetComponentsInChildren<TMP_Dropdown>();
            var dropdownOptions = _tableName.IsEmpty() ? new string[] { "NO TABLE SELECTED" } :
                _dbManager.ConnectedDatabase.Tables[_tableName.Text()].ColumnsNames;

            foreach (var dropdown in dropdowns)
            {
                dropdown.SetOptions(dropdownOptions);
                dropdown.onValueChanged.AddListener(value => Execute());
            }
        }

        public void CreateColumnTypeDropdown()
        {
            var dropdownPrefab = Resources.Load<GameObject>("UI/Shell/Dropdowns/EditDropdown");
            var buttons = _columnsLine.GetComponentsInChildren<Button>();

            Instantiate(_separator, _columnsLine.transform);

            var dropdown = Instantiate(dropdownPrefab, _columnsLine.transform).GetComponent<TMP_Dropdown>();
            SetColumnTypesDropdowns();

            foreach (var button in buttons)
                button.transform.SetAsLastSibling();
            _columnsLine.GetComponentsInChildren<TextMeshProUGUI>()[^5].transform.SetAsLastSibling();

            Instantiate(_separator, _valuesLine.transform);

            var inputFieldPrefab = Resources.Load<GameObject>("UI/Shell/InputFields/EditInputField");
            var inputField = Instantiate(inputFieldPrefab, _valuesLine.transform).GetComponent<TMP_InputField>();
            inputField.onEndEdit.AddListener(value => Execute());
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

        public void Execute()
        {
            var lines = GetComponentsInChildren<Line>();

            var tableNameDropdown = lines[0].GetComponentInChildren<TMP_Dropdown>();
            var columns = _columnsLine.GetComponentsInChildren<TMP_Dropdown>().Select(dropdown => dropdown.Text()).ToArray();
            if (tableNameDropdown.IsEmpty() || columns.Contains("..."))
                return;

            var values = lines[^1].GetComponentsInChildren<TMP_InputField>().Select(inputField => inputField.text).ToArray();
            if (values.Contains(""))
                return;

            _dbManager.InsertInto(gameObject, tableNameDropdown.Text(), columns, values);
        }
    }
}