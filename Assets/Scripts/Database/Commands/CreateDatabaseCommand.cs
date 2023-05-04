using System.Collections.Generic;
using UnityEngine;

namespace SQL_Quest.Database.Commands
{
    public class CreateDatabaseCommand : DatabaseCommand
    {
        private string _name;

        public CreateDatabaseCommand(string name, bool returnMessage = true) : base(returnMessage)
        {
            _name = name;
        }

        public override bool Execute()
        {
            Initialize();
            var database = new Database(_name,
                            $"{Application.dataPath}/Databases/{_dbManager.DatabasesFolder}",
                            new Dictionary<string, Table>());
            _dbManager.ExistingDatabases[_name] = database;
            _dbManager.ExistingDatabases[_name].Connect();
            _dbManager.ExistingDatabases[_name].Disconnect();

            if (!_returnMessage)
                return false;

            SaveBackup();
            _chat.CheckMessage($"CREATE DATABASE {_name}");
            Write("Query OK, 1 row affected");
            return true;
        }

        public new void Undo()
        {
            new DropDatabaseCommand(_name, false).Execute();
            base.Undo();
        }
    }
}