using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SQL_Quest.Components.LevelManagement.CharactersController
{
    [Serializable]
    public class CharactersControllerData
    {
        [SerializeField] private bool _haveMrNormatt;
        [SerializeField] private Vector3 _mrNormattPos;
        [SerializeField] private bool _haveHilpy;
        [SerializeField] private Vector3 _hilpyPos;

        public bool HaveMrNormatt => _haveMrNormatt;
        public Vector3 MrNormattPos => _mrNormattPos;
        public bool HaveHilpy => _haveHilpy;
        public Vector3 HilpyPos => _hilpyPos;
    }
}
