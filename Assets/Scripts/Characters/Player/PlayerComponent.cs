using SQL_Quest.Components.ColliderBased;
using System.Collections;
using UnityEngine;

namespace SQL_Quest.Creatures.Player
{
    public class PlayerComponent : CharacterComponent
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        private void Start()
        {
            var data = PlayerDataHandler.PlayerData;
            transform.position = data.Position;
            UpdateSpriteDirection(new Vector2(data.InvertScale == true ? -1 : 1, 0));

            if (data.InteractOnStart)
                StartCoroutine(InteractOnStart());
        }

        private IEnumerator InteractOnStart()
        {
            yield return new WaitForSeconds(1);
            Interact();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }
    }
}