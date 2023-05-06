using System.Linq;
using UnityEngine;

namespace SQL_Quest.Database.Commands
{
    public class ShowDatabasesCommand : DatabaseCommand
    {
        public ShowDatabasesCommand(bool returnMessage = true) : base(returnMessage)
        {
        }

        public override bool Execute()
        {
            Initialize();
            if (!_returnMessage)
                return false;

            SaveBackup();
            Write(Table.Write("Databases", _dbManager.ExistingDatabases.Keys.ToArray()));
            _chat.CheckMessage("SHOW DATABASES");
            return true;
        }
    }
}