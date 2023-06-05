namespace SQL_Quest.Components.UI.Commands
{
    public class ShowDatabases : UICommand
    {
        protected override void Start()
        {
            base.Start();
            _dbManager.ShowDatabasesCommand(gameObject);
        }
    }
}