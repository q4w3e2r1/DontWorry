using SQL_Quest.Components.ColliderBased;
using SQL_Quest.Extensions;
using System.Collections;
using UnityEngine;

namespace SQL_Quest.Creatures.Player
{
    public class PlayerComponent : CharacterComponent
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        private void Start()
        {
            transform.position = PlayerPrefsExtensions.GetVector3("Position");
            UpdateSpriteDirection(new Vector2(PlayerPrefsExtensions.GetBool("InvertScale") ? -1 : 1, 0));

            if (PlayerPrefsExtensions.GetBool("InteractOnStart"))
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