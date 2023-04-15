using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class Command : MonoBehaviour
{
    protected DatabaseManager _dbManager;
    protected TextMeshProUGUI _output;
    protected string _backup;

    private void Awake()
    {
        _dbManager = GameObject.FindWithTag("DatabaseManager").GetComponent<DatabaseManager>();
        _output = GameObject.FindWithTag("Output").GetComponent<TextMeshProUGUI>();
    }

    protected void SaveBackup()
    { 
        _backup = _output.text;
    }

    public abstract void Undo();

    public void Write(string message)
    {
        _output.text += message + '\n';
        Debug.Log(message);
    }

    public abstract bool Execute();
}
