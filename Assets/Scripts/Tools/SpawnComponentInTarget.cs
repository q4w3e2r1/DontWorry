using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
