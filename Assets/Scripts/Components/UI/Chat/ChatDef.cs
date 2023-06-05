using UnityEngine;

namespace SQL_Quest.Components.UI.Chat
{
    [CreateAssetMenu(fileName = "Chat")]
    public class ChatDef : ScriptableObject
    {
        [SerializeField] private ChatData _data;
        public ChatData Data => _data;
    }
}
