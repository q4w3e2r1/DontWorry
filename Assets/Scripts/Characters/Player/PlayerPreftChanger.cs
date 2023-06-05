using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.EditorTools;
using UnityEngine;

namespace SQL_Quest.Creatures.Player.PlayerPreftChanger
{
    public class PlayerPreftChanger : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private int _levelNum;

        [ContextMenu("ChangeAll")]
        public void ChangeAll()
        { 
            ChangeName();
            ChangeLevelNum();
        }

        public void ChangeName()
        {
            PlayerPrefs.SetString("Name", _name);
        }

        public void ChangeLevelNum()
        {
            PlayerPrefs.SetInt("LevelNumber", _levelNum);
        }
    }
}
