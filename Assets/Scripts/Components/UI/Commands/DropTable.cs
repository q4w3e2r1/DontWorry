using SQL_Quest.Extentions;
using TMPro;
using UnityEngine;

namespace SQL_Quest.UI.Commands
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
            var name = _name.captionText.text;
            if (name == "...")
                return;
            _dbManager.DropTable(gameObject, name);
        }
    }
}