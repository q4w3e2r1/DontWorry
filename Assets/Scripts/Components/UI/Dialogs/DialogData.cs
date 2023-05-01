using System;
using UnityEngine;

namespace SQL_Quest.Components.UI.Dialogs
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private string[] _sentences;
        public string[] Sentences => _sentences;  
    }
}