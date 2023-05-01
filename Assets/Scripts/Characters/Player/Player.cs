using SQL_Quest.Components.ColliderBased;
using UnityEngine;

namespace SQL_Quest.Creatures.Player
{
    public class Player : Creature
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        protected override void Awake()
        {
            base.Awake();
            transform.position = _data.Position;
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }
    }
}