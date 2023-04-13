using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public class RedoCommand : Command
{
    private void Start()
    {
        //GetComponent<Button>().onClick.AddListener(value => _dbManager.ExecuteCommand(this));
    }

    public override bool Execute()
    {
        _dbManager.Redo();
        return false;
    }
}
