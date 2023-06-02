using SQL_Quest.Extentions;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SQL_Quest.UI.Commands
{
    public class CreateDatabase : UICommand
    {
        [SerializeField] private TMP_Dropdown _name;

        protected override void Start()
        {
            base.Start();
            _name.SetOptions(_dbManager.AllowedDatabases.Keys.ToArray());
            _name.onValueChanged.AddListener(value => Execute());
        }

        private void Execute()
        {
            if (_name.IsEmpty())
                return;
            _dbManager.CreateDatabase(gameObject, _name.Text());
        }
    }
}