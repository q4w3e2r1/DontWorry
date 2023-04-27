using System;
using UnityEngine.Events;

namespace SQL_Quest.Components.UI.Chat
{
    [Serializable]
    public class Message
    {
        public string Text;
        public string ConditionsForSending;
        public bool IsSent;
        public UnityEvent OnSending;
    }
}