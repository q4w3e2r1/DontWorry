using Scripts.Extensions;
using TMPro;
using UnityEngine;

public class UseDatabaseCommand : Command
{
    [SerializeField] private TMP_Dropdown _name;
    private Database _databaseBackup;

    private void Start()
    {
        _name.SetOptions(_dbManager.ExistingDatabases);
        _name.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));
    }

    public override bool Execute()
    {
        var name = _name.captionText.text;
        if (name == "...")
            return false;

        SaveBackup();

        _dbManager.UseDatabase(name);
        Write("Database changed");

        return true;
    }

    private new void SaveBackup()
    {
        _databaseBackup = _dbManager.ConnectedDatabase;
        base.SaveBackup();
    }

    public override void Undo()
    {
        if (_databaseBackup != null)
            _dbManager.UseDatabase(_databaseBackup.Name);
        else
            _dbManager.UseDatabase("");

        _output.text = _backup;
        Destroy(gameObject);
    }
}