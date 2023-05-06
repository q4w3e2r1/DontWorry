using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SQL_Quest.Components.Interactables
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            _action?.Invoke();
        }
    }
}