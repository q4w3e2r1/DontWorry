using SQL_Quest.Extentions;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SQL_Quest.Components.UI.Commands
{
    public class DropDatabase : UICommand
    {
        [SerializeField] private TMP_Dropdown _name;

        protected override void Start()
        {
            base.Start();
            _name.SetOptions(_dbManager.ExistingDatabases.Keys.ToArray());
            _name.onValueChanged.AddListener(value => Execute());
        }

        private void Execute()
        {
            if (_name.IsEmpty())
                return;
            _dbManager.DropDatabaseCommand(gameObject, _name.GetText());
        }
    }
}