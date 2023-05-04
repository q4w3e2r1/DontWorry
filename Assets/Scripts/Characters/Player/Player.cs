using SQL_Quest.Components.ColliderBased;
using UnityEngine;

namespace SQL_Quest.Creatures.Player
{
    public class Player : Creature
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        protected override void Start()
        {
            base.Start();
            transform.position = _data.Position;
            UpdateSpriteDirection(new Vector2(_data.InvertScale == true ? -1 : 1, 0));

            if (_data.InteractOnStart)
                Interact();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }
    }
}