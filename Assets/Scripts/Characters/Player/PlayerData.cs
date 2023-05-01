using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SQL_Quest.Creatures.Player
{
    [CreateAssetMenu]
    [Serializable]
    public class PlayerData : ScriptableObject
    {
        public string Name;
        public Vector3 Position;
    }
}