using SQL_Quest.Extentions;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SQL_Quest.Components.UI.Commands
{
    public class Update : UICommand
    {
        [SerializeField] private TMP_Dropdown _tableName;
        [SerializeField] private UpdateLine _setLine;
        [SerializeField] private UpdateLine _whereLine;

        protected override void Start()
        {
            base.Start();
            if (_dbManager.ConnectedDatabase == null)
            {
                _dbManager.UpdateCommand(gameObject, "", "", "");
                return;
            }

            var columns = _dbManager.ConnectedDatabase.Tables.Keys.ToArray();
            _tableName.SetOptions(columns);
            _tableName.AddListeners(value => UpdateValue(), value => Execute());

            SetLine(_setLine);
            SetLine(_whereLine);
        }

        private void SetLine(UpdateLine line)
        {
            var expressions = new string[] { "=", "!=", "<", "<=", ">", ">=" };
            line.ColumnType.AddListeners(value => Execute());
            line.Expression.SetOptions(expressions);
            line.Expression.AddListeners(value => Execute());
            line.InputField.onEndEdit.AddListener(value => Execute());
        }

        private void UpdateValue()
        {
            if (_tableName.IsEmpty())
                return;

            var tableColumns = _dbManager.ConnectedDatabase.Tables[_tableName.GetText()].ColumnsNames;
            _setLine.ColumnType.SetOptions(tableColumns);
            _whereLine.ColumnType.SetOptions(tableColumns);
        }

        public void Execute()
        {
            if (_tableName.IsEmpty() || IsLineEmpty(_setLine) || IsLineEmpty(_whereLine))
                return;

            _dbManager.UpdateCommand(gameObject, _tableName.GetText(), _setLine.ToString(), _whereLine.ToString());
        }

        private bool IsLineEmpty(UpdateLine line)
        {
            return line.ColumnType.IsEmpty() || line.Expression.IsEmpty() || line.InputField.text == "";
        }
    }

    [Serializable]
    public class UpdateLine
    {
        public TMP_Dropdown ColumnType;
        public TMP_Dropdown Expression;
        public TMP_InputField InputField;

        public override string ToString()
        {
            return $"{ColumnType.GetText()} {Expression.GetText()} \"{InputField.text}\"";
        }
    }
}
