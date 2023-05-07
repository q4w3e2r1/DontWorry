using SQL_Quest.Extentions;
using SQL_Quest.UI.Commands;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SQL_Quest.Database.Commands
{
    public class Select : UICommand
    {
        [SerializeField] private TMP_Dropdown _tableName;
        [SerializeField] private TMP_Dropdown _selectedValue;

        protected new void Start()
        {
            base.Start();
            var columns = _dbManager.ConnectedDatabase.Tables.Keys.ToArray();
            _tableName.SetOptions(columns);
            _tableName.onValueChanged.AddListener(value => Execute());

            var selectedValueOptions = new string[] { "*" };
            _selectedValue.SetOptions(selectedValueOptions);
            _selectedValue.onValueChanged.AddListener(value => Execute());
        }

        public void UpdateSelectedValue()
        {
            if (_tableName.captionText.text == "...")
                return;

            var columns = _dbManager.ConnectedDatabase.Tables[_tableName.captionText.text].ColumsDictionary.Keys.ToArray();
            var selectedValueOptions = new List<string> { "*" };
            foreach (var column in columns)
                selectedValueOptions.Add(column);
            _selectedValue.SetOptions(selectedValueOptions.ToArray());
        }

        public void Execute()
        {
            var tableName = _tableName.captionText.text;
            var selectedValue = _selectedValue.captionText.text;

            if (tableName == "..." || selectedValue == "...")
                return;

            _dbManager.Select(tableName, selectedValue);
        }
    }
}