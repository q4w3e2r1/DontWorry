using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        _output.GetComponentInParent<ScrollRect>().ScrollToBottom();
    }

    public abstract bool Execute();
}
