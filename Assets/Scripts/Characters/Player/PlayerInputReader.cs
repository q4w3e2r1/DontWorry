using UnityEngine;
using UnityEngine.InputSystem;

namespace SQL_Quest.Creatures.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private Player _hero;

        public void OnMove(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
                _hero.Interact();
        }
    }
}