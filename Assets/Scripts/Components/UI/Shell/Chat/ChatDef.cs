using UnityEngine;

namespace SQL_Quest.Components.UI.Shell.Chat
{
    [CreateAssetMenu(fileName = "Chat")]
    public class ChatDef : ScriptableObject
    {
        [SerializeField] private ChatData _data;
        public ChatData Data => _data;
    }
}
