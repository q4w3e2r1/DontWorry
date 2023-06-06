using UnityEngine;

namespace SQL_Quest.Components.LevelManagement.CharactersController
{
    [CreateAssetMenu(fileName = "CharactersController")]
    public class CharactersControllerDef : ScriptableObject
    {
        [SerializeField] private CharactersControllerData _data;
        public CharactersControllerData Data => _data;
    }
}
