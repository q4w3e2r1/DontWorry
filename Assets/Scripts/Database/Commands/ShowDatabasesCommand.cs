public class ShowDatabasesCommand : Command
{
    public ShowDatabasesCommand(ShowDatabasesCommand command) { }

    public ShowDatabasesCommand Copy(ShowDatabasesCommand command)
        => new ShowDatabasesCommand(command);

    private void Start()
    {
        _dbManager.ExecuteCommand(this);
    }

    public override bool Execute()
    {
        SaveBackup();

        Write(_dbManager.ShowDatabases());

        return true;
    }

    public override void Undo()
    {
        _output.text = _backup;
        Destroy(gameObject);
    }
}