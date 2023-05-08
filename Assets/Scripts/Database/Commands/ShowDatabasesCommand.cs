using System.Linq;

namespace SQL_Quest.Database.Commands
{
    public class ShowDatabasesCommand : DatabaseCommand
    {
        protected override void SaveBackup()
        {
            base.SaveBackup();
        }

        public override bool Execute()
        {
            if (!_returnMessage)
                return false;

            SaveBackup();
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