using System;
using UnityEngine;

namespace SQL_Quest.Components.UI.Dialogs
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private Sentence[] _sentences;
        public Sentence[] Sentences => _sentences;
    }

    [Serializable]
    public struct Sentence
    {
        [SerializeField] private string _name;
        [SerializeField] private string _value;
        [SerializeField] private AudioClip _voice;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Side _side;

        public string Name => _name;
        public string Value => _value;
        public AudioClip Voice => _voice;
        public Sprite Icon => _icon;
        public Side Side => _side;
    }

    public enum Side
    {
        Left,
        Right
    }
}