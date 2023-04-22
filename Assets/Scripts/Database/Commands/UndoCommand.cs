public class UndoCommand : DatabaseCommand
{
    public override bool Execute()
    {
        _dbManager.Undo();
        return false;
    }

    public override void Undo()
    {
        _output.text = _backup;
        Destroy(gameObject);
    }
}
