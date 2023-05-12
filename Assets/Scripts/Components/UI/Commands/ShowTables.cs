namespace SQL_Quest.UI.Commands
{
    public class ShowTables : UICommand
    {
        protected override void Start()
        {
            base.Start();
            _dbManager.ShowTables(gameObject);
        }
    }
}