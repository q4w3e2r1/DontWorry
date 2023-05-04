using System;
using UnityEngine;

namespace SQL_Quest.Components.UI.Dialogs
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private Sentence[] _sentences;
        [SerializeField] private DialogType _type;
        public Sentence[] Sentences => _sentences;
        public DialogType Type => _type;
    }

    [Serializable]
    public struct Sentence
    {
        [SerializeField] private string _name;
        [SerializeField] private string _value;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Side _side;

        public string Name => _name;
        public string Value => _value;
        public Sprite Icon => _icon;
        public Side Side => _side;
    }

    public enum Side
    {
        Left,
        Right
    }

    public enum DialogType
    {
        Simple,
        Personalized
    }
}