
using SQL_Quest.UI.Commands;

namespace SQL_Quest.Database.Commands
{
    public class ShowDatabases : UICommand
    {
        private new void Start()
        {
            base.Start();
            _dbManager.ShowDatabases();
        }
    }
}