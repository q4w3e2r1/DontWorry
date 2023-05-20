using UnityEngine;
using UnityEngine.InputSystem;

namespace SQL_Quest.Creatures.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private PlayerComponent _player;

        private bool _active = true;

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!_active)
                return;
            var direction = context.ReadValue<Vector2>();
            _player.SetDirection(direction);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!_active)
                return;
            if (context.canceled)
                _player.Interact();
        }

        private void OnEnable()
        {
            _active = true;
        }

        private void OnDisable()
        {
            _active = false;
        }
    }
}