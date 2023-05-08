using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
