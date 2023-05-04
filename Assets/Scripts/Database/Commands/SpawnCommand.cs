using SQL_Quest.GoBased;
using UnityEngine;

namespace SQL_Quest.Database.Commands
{
    public class SpawnCommand : DatabaseCommand
    {
        [SerializeField] private SpawnComponentInTarget _spawner;

        private GameObject _gameObject;

        private void Start()
        {
            _dbManager = GameObject.FindWithTag("DatabaseManager").GetComponent<DatabaseManager>();
        }

        public void OnClick()
        {
            _dbManager.ExecuteCommand(this);
        }

        public override bool Execute()
        {
            _gameObject = _spawner.Spawn();
            return true;
        }

        public new void Undo()
        {
            Destroy(_gameObject);
        }
    }
}