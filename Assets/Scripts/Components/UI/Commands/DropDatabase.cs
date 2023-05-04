using SQL_Quest.Extentions;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SQL_Quest.UI.Commands
{
    public class DropDatabase : UICommand
    {
        [SerializeField] private TMP_Dropdown _name;

        private new void Start()
        {
            base.Start();
            _name.SetOptions(_dbManager.ExistingDatabases.Keys.ToArray());
            _name.onValueChanged.AddListener(value => Execute());
        }

        private void Execute()
        {
            var name = _name.captionText.text;
            if (name == "...")
                return;
            _dbManager.DropDatabase(name);
        }
    }
}