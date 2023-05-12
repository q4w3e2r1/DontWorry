namespace SQL_Quest.UI.Commands
{
    public class ShowDatabases : UICommand
    {
        protected override void Start()
        {
            base.Start();
            _dbManager.ShowDatabases(gameObject);
        }
    }
}