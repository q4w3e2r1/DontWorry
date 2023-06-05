using SQL_Quest.Extentions;
using TMPro;
using UnityEngine;

namespace SQL_Quest.Components.UI.Commands
{
    public class DropTable : UICommand
    {
        [SerializeField] private TMP_Dropdown _name;

        protected override void Start()
        {
            base.Start();
            _name.SetOptions(_dbManager.GetTables());
            _name.onValueChanged.AddListener(value => Execute());
        }

        private void Execute()
        {
            if (_name.IsEmpty())
                return;
            _dbManager.DropTableCommand(gameObject, _name.GetText());
        }
    }
}