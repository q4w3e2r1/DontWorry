
using SQL_Quest.UI.Commands;

namespace SQL_Quest.Database.Commands
{
    public class ShowDatabases : UICommand
    {
        protected new void Start()
        {
            base.Start();
            _dbManager.ShowDatabases(gameObject);
        }
    }
}