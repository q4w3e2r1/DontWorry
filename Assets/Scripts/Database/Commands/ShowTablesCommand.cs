public class ShowTablesCommand : Command
{
    private void Start()
    {
        _dbManager.ExecuteCommand(this);
    }

    public override bool Execute()
    {
        if (_dbManager.ConnectedDatabase == null)
        {
            Write("ERROR 1046 (3D000): No database selected");
            return true;
        }

        SaveBackup();

        Write(_dbManager.ShowTables());

        return true;
    }

    public override void Undo()
    {
        _output.text = _backup;
        Destroy(gameObject);
    }
}