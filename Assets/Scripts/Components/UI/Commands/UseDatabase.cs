using SQL_Quest.Extentions;
using SQL_Quest.UI.Commands;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SQL_Quest.Database.Commands
{
    public class UseDatabase : UICommand
    {
        [SerializeField] private TMP_Dropdown _name;

        private new void Start()
        {
            base.Start();
            _name.SetOptions(_dbManager.ExistingDatabases.Keys.ToArray());
            _name.onValueChanged.AddListener(value => Execute());
        }

        public void Execute()
        {
            var name = _name.captionText.text;
            if (name == "...")
                return;
            _dbManager.UseDatabase(name);
        }
    }
}