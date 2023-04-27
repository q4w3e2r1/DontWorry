using SQL_Quest.GoBased;
using UnityEngine;

namespace SQL_Quest.Database.Commands
{
    public class SpawnCommand : DatabaseCommand
    {
        [SerializeField] private SpawnComponentInTarget _spawner;

        private GameObject _gameObject;

        public void OnClick()
        {
            _dbManager.ExecuteCommand(this);
        }

        public override bool Execute()
        {
            _gameObject = _spawner.Spawn();
            return true;
        }

        public override void Undo()
        {
            Destroy(_gameObject);
        }
    }
}