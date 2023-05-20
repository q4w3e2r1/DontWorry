using SQL_Quest.Components.UI;
using SQL_Quest.Components.UI.Line;
using SQL_Quest.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SQL_Quest.UI.Commands
{
    public class Select : UICommand
    {
        [SerializeField] private TMP_Dropdown _tableName;

        private Line _firstLine;
        private List<TMP_Dropdown> _selectedValues;
        private List<GameObject> _additionalSelection;
        private AdditionalSelectionData _additionalSelectionData;

        private GameObject _textPrefab;
        private GameObject _dropdownPrefab;
        private GameObject _inputFieldPrefab;

        protected override void Start()
        {
            base.Start();
            if (_dbManager.ConnectedDatabase == null)
            {
                _dbManager.Select(gameObject, "", "", "");
                return;
            }

            _textPrefab = Resources.Load<GameObject>("UI/Text");
            _dropdownPrefab = Resources.Load<GameObject>("UI/Commands/Dropdowns/CreateDropdown");
            _inputFieldPrefab = Resources.Load<GameObject>("UI/Commands/InputFields/CreateInputField");

            _firstLine = GetComponentInChildren<Line>();

            var columns = _dbManager.ConnectedDatabase.Tables.Keys.ToArray();
            _tableName.SetOptions(columns);
            _tableName.AddListeners(value => UpdateSelectedValue(), value => Execute());

            _selectedValues = new() { _firstLine.GetComponentsInChildren<TMP_Dropdown>()[0] };
            _selectedValues[0].SetOptions(AdditionalSelection.Values.Keys.ToArray(), true);
            _selectedValues[0].AddListeners(value => UpdateSelectedValue(), value => Execute());
        }

        public void UpdateSelectedValue()
        {
            CheckFirstValue();

            if (_tableName.captionText.text == "...")
                return;

            var capturedText = _selectedValues.Select(dropdown => dropdown.Text()).ToArray();

            var columns = _dbManager.ConnectedDatabase.Tables[_tableName.Text()].ColumnsNames;
            _selectedValues[0].SetOptions(columns.Concat(AdditionalSelection.Values.Keys).ToArray(), true);
            _selectedValues[0].captionText.text = capturedText[0];

            if (_additionalSelectionData == null)
                return;

            _selectedValues[1].SetOptions(columns, _additionalSelectionData.HaveAllOption);
            _selectedValues[1].captionText.text = capturedText[1];
        }

        private void CheckFirstValue()
        {
            var firstValue = _selectedValues[0].Text();
            if (!AdditionalSelection.Values.ContainsKey(firstValue) && _additionalSelection == null)
                return;

            if (!AdditionalSelection.Values.ContainsKey(firstValue) && _additionalSelection != null)
            {
                DropAdditionalSelection();
                return;
            }

            var additionalSelectionData = AdditionalSelection.Values[firstValue];
            if (_additionalSelectionData == null)
            {
                CreateAdditionalSelection(additionalSelectionData);
                return;
            }

            if (additionalSelectionData == _additionalSelectionData)
                return;

            DropAdditionalSelection();
            CreateAdditionalSelection(additionalSelectionData);
        }

        private void CreateAdditionalSelection(AdditionalSelectionData data)
        {
            while (_selectedValues.Count != 1)
                DestroyColumnType();

            var columnNameDropdown = Instantiate(_dropdownPrefab, _firstLine.transform).GetComponent<TMP_Dropdown>();
            SetColumnsOption(columnNameDropdown, data.HaveAllOption);
            columnNameDropdown.AddListeners(value => UpdateSelectedValue(), value => Execute());

            var openBracket = Instantiate(_textPrefab, _firstLine.transform);
            openBracket.GetComponent<TextMeshProUGUI>().text = "(";

            var closingBracket = Instantiate(_textPrefab, _firstLine.transform);
            closingBracket.GetComponent<TextMeshProUGUI>().text = ")";

            _additionalSelection = new List<GameObject>() { openBracket, columnNameDropdown.gameObject, closingBracket };
            for (int i = 0; i < _additionalSelection.Count; i++)
                _additionalSelection[i].transform.SetSiblingIndex(i + 2);
            ChangeBracketsState(data.HaveBrackets);

            _firstLine.GetComponentInChildren<Modifier>(true).gameObject.SetActive(data.HideModifier);
            _selectedValues.Add(columnNameDropdown);
            _additionalSelectionData = data;
        }
        private void ChangeBracketsState(bool state)
        {
            _additionalSelection[0].SetActive(state);
            _additionalSelection[^1].SetActive(state);
        }

        private void DropAdditionalSelection()
        {
            while (_selectedValues.Count != 1)
                DestroyColumnType();

            foreach (var item in _additionalSelection)
                Destroy(item);
            _additionalSelection.Clear();
            _additionalSelectionData = null;
        }

        private void SetColumnsOption(TMP_Dropdown dropdown, bool haveAllOption)
        {
            if (_tableName.IsEmpty())
                dropdown.SetOptions(Array.Empty<string>(), haveAllOption);
            else
            {
                var columns = _dbManager.ConnectedDatabase.Tables[_tableName.Text()].ColumnsNames;
                dropdown.SetOptions(columns, haveAllOption);
            }
        }

        public void CreateColumnType()
        {
            var separator = Instantiate(_textPrefab, _firstLine.transform).GetComponent<TextMeshProUGUI>();
            separator.text = ",";
            separator.transform.SetSiblingIndex(_selectedValues.Count * 2);

            var columnTypeDropdown = Instantiate(_dropdownPrefab, _firstLine.transform).GetComponent<TMP_Dropdown>();
            SetColumnsOption(columnTypeDropdown, _additionalSelectionData == null || _additionalSelectionData.HaveAllOption);
            columnTypeDropdown.AddListeners(value => UpdateSelectedValue(), value => Execute());
            _selectedValues.Add(columnTypeDropdown);
            columnTypeDropdown.transform.SetSiblingIndex(_selectedValues.Count * 2 - 1);
        }

        public void DestroyColumnType()
        {
            Destroy(_firstLine.transform.GetChild(_selectedValues.Count * 2 - 1).gameObject);
            Destroy(_firstLine.transform.GetChild((_selectedValues.Count - 1) * 2).gameObject);
            _selectedValues.RemoveAt(_selectedValues.Count - 1);
        }


        #region AdditionalLine
        public void SetLine()
        {
            var line = GetComponentsInChildren<Line>()[^1];
            var dropdown = line.GetComponentInChildren<TMP_Dropdown>();
            dropdown.SetOptions(new string[] { "WHERE", "ORDER BY" });
            dropdown.AddListeners(value => UpdateLine(), value => Execute());
        }

        public void UpdateLine()
        {
            var line = GetComponentsInChildren<Line>()[^1];
            var dropdowns = line.GetComponentsInChildren<TMP_Dropdown>();
            var inputFields = line.GetComponentsInChildren<TMP_InputField>();

            switch (dropdowns[0].Text())
            {
                case "...":
                    UpdateDefaultLine(dropdowns, inputFields);
                    break;
                case "WHERE":
                    UpdateWhereLine(line, dropdowns, inputFields);
                    break;
                case "ORDER BY":
                    UpdateOrderByLine(line, dropdowns, inputFields);
                    break;
            }
        }

        private void UpdateDefaultLine(TMP_Dropdown[] dropdowns, TMP_InputField[] inputFields)
        {
            if (dropdowns.Length == 1 && inputFields.Length == 0)
                return;
            CleanLine(dropdowns, inputFields);
            return;
        }

        private void UpdateWhereLine(Line line, TMP_Dropdown[] dropdowns, TMP_InputField[] inputFields)
        {
            if (dropdowns.Length == 3 && inputFields.Length == 1)
                return;

            CleanLine(dropdowns, inputFields);

            var columnTypeDropdown = Instantiate(_dropdownPrefab, line.transform).GetComponent<TMP_Dropdown>();
            SetColumnsOption(columnTypeDropdown, false);
            columnTypeDropdown.AddListeners(value => UpdateLine(), value => Execute());

            var expressionDropdown = Instantiate(_dropdownPrefab, line.transform).GetComponent<TMP_Dropdown>();
            expressionDropdown.SetOptions(new string[] { "=", "!=", "<", "<=", ">", ">=" });
            expressionDropdown.AddListeners(value => Execute());

            var inputField = Instantiate(_inputFieldPrefab, line.transform).GetComponent<TMP_InputField>();
            inputField.onEndEdit.AddListener(value => Execute());
        }

        private void UpdateOrderByLine(Line line, TMP_Dropdown[] dropdowns, TMP_InputField[] inputFields)
        {
            if (dropdowns.Length == 2 && inputFields.Length == 0)
                return;

            CleanLine(dropdowns, inputFields);

            var columnTypeDropdown = Instantiate(_dropdownPrefab, line.transform).GetComponent<TMP_Dropdown>();
            SetColumnsOption(columnTypeDropdown, false);
            columnTypeDropdown.AddListeners(value => Execute());
        }

        private void CleanLine(TMP_Dropdown[] dropdowns, TMP_InputField[] inputFields)
        {
            for (int i = dropdowns.Length - 1; i > 0; i--)
                Destroy(dropdowns[i].gameObject);

            for (int i = inputFields.Length - 1; i >= 0; i--)
                Destroy(inputFields[i].gameObject);
        }

        #endregion

        public void Execute()
        {
            var firstLineDropdowns = _firstLine.GetComponentsInChildren<TMP_Dropdown>()
                .Select(dropdown => dropdown.captionText.text)
                .ToArray();
            if (firstLineDropdowns.Any(dropdownText => dropdownText == "..."))
                return;

            var tableName = firstLineDropdowns[^1];

            var selectedValue = _selectedValues[0].Text();
            if (_additionalSelectionData != null)
            {
                selectedValue = _additionalSelectionData.HaveBrackets ?
                    $"{_selectedValues[0].Text()}({_selectedValues[1].Text()})" :
                    $"{_selectedValues[0].Text()} {_selectedValues[1].Text()}";
            }

            if (GetComponentsInChildren<Line>().Length == 1)
            {
                _dbManager.Select(gameObject, tableName, selectedValue, "",
                    _additionalSelectionData == null || _additionalSelectionData.WriteManyColumns);
                return;
            }

            var secondLine = GetComponentsInChildren<Line>()[1];
            var secondLineDropdowns = secondLine.GetComponentsInChildren<TMP_Dropdown>()
                .Select(dropdown => dropdown.captionText.text)
                .ToArray();
            if (secondLineDropdowns.Any(dropdownText => dropdownText == "..."))
                return;

            string filter = "";
            switch (secondLineDropdowns[0])
            {
                case "WHERE":
                    var inputField = secondLine.GetComponentInChildren<TMP_InputField>().text;
                    if (inputField == "")
                        return;

                    filter = $" {string.Join(" ", secondLineDropdowns)} \"{inputField}\"";
                    break;
                case "ORDER BY":
                    filter = $" {string.Join(" ", secondLineDropdowns)}";
                    break;
            }
            _dbManager.Select(gameObject, tableName, selectedValue, filter,
                    _additionalSelectionData == null || _additionalSelectionData.WriteManyColumns);
        }
    }

    public class AdditionalSelectionData
    {
        public bool HaveBrackets { get; set; }
        public bool HaveAllOption { get; set; }
        public bool HideModifier { get; set; }
        public bool WriteManyColumns { get; set; }

        public AdditionalSelectionData(
            bool haveBracketsInAdditionalSelection,
            bool haveAllOption,
            bool hideSelectedValueModifier,
            bool writeManyColumns)
        {
            HaveBrackets = haveBracketsInAdditionalSelection;
            HaveAllOption = haveAllOption;
            HideModifier = hideSelectedValueModifier;
            WriteManyColumns = writeManyColumns;
        }

        public static bool operator ==(AdditionalSelectionData first, AdditionalSelectionData second)
        {
            return ReferenceEquals(first, second);
        }

        public static bool operator !=(AdditionalSelectionData first, AdditionalSelectionData second)
        {
            return !ReferenceEquals(first, second);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HaveBrackets, HaveAllOption, HideModifier, WriteManyColumns);
        }
    }

    public static class AdditionalSelection
    {
        public static Dictionary<string, AdditionalSelectionData> Values = new()
        {
            ["MAX"] = new(true, false, true, false),
            ["MIN"] = new(true, false, true, false),
            ["DISTINCT"] = new(false, true, false, true),
            ["COUNT"] = new(true, false, true, false)
        };
    }
}