using System;
using UnityEngine;

namespace SQL_Quest.Database.Manager
{
    [Serializable]
    public class DatabaseManagerData
    {
        [SerializeField] private string _databaseFolder;
        [SerializeField] private Table[] _tables;
        [Space]
        [SerializeField] private string[] _allowedColumnTypes;
        [SerializeField] private string[] _allowedColumnAttributes;

        public string DatabaseFolder => _databaseFolder;
        public Table[] Tables => _tables;
        public string[] AllowedColumnTypes => _allowedColumnTypes;
        public string[] AllowedColumnAttributes => _allowedColumnAttributes;
    }
}
