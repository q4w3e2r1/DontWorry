using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

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
