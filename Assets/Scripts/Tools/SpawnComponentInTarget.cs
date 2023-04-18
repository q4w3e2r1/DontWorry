using UnityEngine;

public class SpawnComponentInTarget : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _prefab;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        var go = Instantiate(_prefab, _target.transform);
    }
}
