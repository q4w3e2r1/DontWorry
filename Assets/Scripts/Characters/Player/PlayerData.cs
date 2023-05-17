using System;
using UnityEngine;

namespace SQL_Quest.Creatures.Player
{
    [CreateAssetMenu]
    [Serializable]
    public class PlayerData : ScriptableObject
    {
        public string Name;
        public int LevelNumber;
        public Vector3 Position;
        public bool InvertScale;
        public bool InteractOnStart;
    }
}