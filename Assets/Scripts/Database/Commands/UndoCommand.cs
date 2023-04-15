using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UndoCommand : Command
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
