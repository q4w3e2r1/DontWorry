using System.Collections.Generic;
using UnityEngine;

namespace SQL_Quest.Database.Commands
{
    public class CreateDatabaseCommand : DatabaseCommand
    {
        private string _name;

        public void Constructor(string name, bool returnMessage = true)
        {
            _name = name;
            Constructor(CommandType.Simple, returnMessage);
        }

        public override bool Execute()
        {
            base.Execute();
            SaveBackup();

            var database = new Database(_name,
                            $"{Application.dataPath}/Databases/{_dbManager.DatabasesFolder}",
                            new Dictionary<string, Table>());
            _dbManager.ExistingDatabases[_name] = database;

            if (!_returnMessage)
                return false;

            database.Connect();
            database.Disconnect();

            _chat.CheckMessage($"CREATE DATABASE {_name}");
            Write("Query OK, 1 row affected");
            return true;
        }

        public override void Undo()
        {
            var undoCommand = gameObject.AddComponent<DropDatabaseCommand>();
            undoCommand.Constructor(_name, false);
            undoCommand.Execute();
            Destroy(undoCommand);
            base.Undo();
        }
    }
}