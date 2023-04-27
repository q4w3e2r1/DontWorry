
namespace SQL_Quest.Database.Commands
{
    public class RedoCommand : DatabaseCommand
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
}