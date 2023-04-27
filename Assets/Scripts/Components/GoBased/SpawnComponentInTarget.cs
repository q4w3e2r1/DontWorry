using UnityEngine;

namespace SQL_Quest.GoBased
{
    public class SpawnComponentInTarget : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public GameObject Spawn()
        {
            return Instantiate(_prefab, _target.transform);
        }
    }
}