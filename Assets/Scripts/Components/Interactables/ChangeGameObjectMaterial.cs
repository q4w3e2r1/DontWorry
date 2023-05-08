using UnityEngine;

namespace SQL_Quest.Components.Interactables
{
    public class ChangeGameObjectMaterial : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Material _newMaterial;

        public void Change()
        {
            _gameObject.GetComponent<SpriteRenderer>().material = _newMaterial;
        }
    }
}
