using Scripts.Extensions;
using TMPro;
using UnityEngine;

public class DropDatabaseCommand : DatabaseCommand
{
    [SerializeField] private TMP_Dropdown _name;

    private void Start()
    {
        _name.SetOptions(_dbManager.ExistingDatabases);
        _name.onValueChanged.AddListener(value => _dbManager.ExecuteCommand(this));
    }

    public override bool Execute()
    {
        var databaseName = _name.captionText.text;
        if (databaseName == "...")
            return false;

        SaveBackup();

        _dbManager.DropDatabase(databaseName);
        Write("Query OK, 0 row affected");

        return true;
    }

    public override void Undo()
    {
        _dbManager.CreateDatabase(_name.captionText.text);

        _output.text = _backup;
        Destroy(gameObject);
    }
}