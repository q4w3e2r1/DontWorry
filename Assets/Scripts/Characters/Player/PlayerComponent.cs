using SQL_Quest.Components.ColliderBased;
using UnityEngine;

namespace SQL_Quest.Creatures.Player
{
    public class PlayerComponent : CharacterComponent
    {
        public PlayerData Data;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        protected override void Start()
        {
            base.Start();
            transform.position = Data.Position;
            UpdateSpriteDirection(new Vector2(Data.InvertScale == true ? -1 : 1, 0));

            if (Data.InteractOnStart)
                Interact();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }
    }
}