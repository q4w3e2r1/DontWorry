namespace SQL_Quest.Components.UI.Commands
{
    public class ShowTables : UICommand
    {
        protected override void Start()
        {
            base.Start();
            _dbManager.ShowTablesCommand(gameObject);
        }
    }
}