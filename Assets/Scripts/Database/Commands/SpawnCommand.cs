using SQL_Quest.GoBased;
using UnityEngine;

namespace SQL_Quest.Database.Commands
{
    public class SpawnCommand : DatabaseCommand
    {
        private SpawnComponentInTarget _spawner;

        private GameObject _gameObject;

        public void Constructor(SpawnComponentInTarget spawner)
        {
            Type = CommandType.Complex;
            _spawner = spawner;
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