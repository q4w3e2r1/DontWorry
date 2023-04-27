using SQL_Quest.Components.ColliderBased;
using UnityEngine;

namespace SQL_Quest.Creatures
{
    public class Hero : Creature
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }
    }
}