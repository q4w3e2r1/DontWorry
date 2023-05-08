using System.Linq;

namespace SQL_Quest.Database.Commands
{
    public class ShowDatabasesCommand : DatabaseCommand
    {
        public override bool Execute()
        {
            base.Execute();
            SaveBackup();

            if (!_returnMessage)
                return false;
                        
            Write(Table.Write("Databases", _dbManager.ExistingDatabases.Keys.ToArray()));
            _chat.CheckMessage("SHOW DATABASES");
            return true;
        }

        public override void Undo()
        {
            base.Undo();
        }
    }
}