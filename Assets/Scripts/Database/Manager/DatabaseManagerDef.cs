using UnityEngine;

namespace SQL_Quest.Database.Manager
{
    [CreateAssetMenu]
    public class DatabaseManagerDef : ScriptableObject
    {
        [SerializeField] private DatabaseManagerData _data;
        public DatabaseManagerData Data => _data;
    }
}
