public class RedoCommand : Command
{
    public override bool Execute()
    {
        _dbManager.Redo();
        return false;
    }

    public override void Undo()
    {
        _output.text = _backup;
        Destroy(gameObject);
    }
}
