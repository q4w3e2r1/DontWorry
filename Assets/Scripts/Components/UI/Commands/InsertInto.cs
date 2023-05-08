using SQL_Quest.Components.UI.Line;
using SQL_Quest.Extentions;
using SQL_Quest.UI.Commands;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Database.Commands
{
    public class InsertInto : UICommand
    {
        [SerializeField] private GameObject _textPrefab;
        private TMP_Dropdown _tableName;
        private GameObject _separator;
        private Line _columnsLine;
        private Line _valuesLine;

        protected new void Start()
        {
            base.Start();
            if (_dbManager.ConnectedDatabase == null)
            {
                _dbManager.Select(gameObject, "", "");
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
            _valuesLine.GetComponentInChildren<TMP_InputField>().onEndEdit.AddListener(value => Execute());
        }

        public void SetColumnTypesDropdowns()
        {
            var dropdowns = _columnsLine.GetComponentsInChildren<TMP_Dropdown>();
            var dropdownOptions = _tableName.captionText.text == "..." ? new string[] { "NO TABLE SELECTED" } :
                _dbManager.ConnectedDatabase.Tables[_tableName.captionText.text].Columns;

            foreach (var dropdown in dropdowns)
            {
                dropdown.SetOptions(dropdownOptions);
                dropdown.onValueChanged.AddListener(value => Execute());
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

            var tableName = lines[0].GetComponentInChildren<TMP_Dropdown>().captionText.text;
            var columns = _columnsLine.GetComponentsInChildren<TMP_Dropdown>().Select(dropdown => dropdown.captionText.text).ToArray();
            if (tableName == "..." || columns.Contains("..."))
                return;

            var values = lines[^1].GetComponentsInChildren<TMP_InputField>().Select(inputField => inputField.text).ToArray();
            if (values.Contains(""))
                return;

            _dbManager.InsertInto(gameObject, tableName, columns, values);
        }
    }
}