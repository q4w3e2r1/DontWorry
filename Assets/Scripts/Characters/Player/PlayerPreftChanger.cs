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

        [ContextMenu("ChangeName")]
        public void ChangeName()
        {
            PlayerPrefs.SetString("Name", _name);
        }

        [ContextMenu("ChangeLevelNum")]
        public void ChangeLevelNum()
        {
            PlayerPrefs.SetInt("LevelNumber", _levelNum);
        }

        [ContextMenu("DeleteAll")]
        public void DeleteAll()
        {
            PlayerPrefs.DeleteKey("Name");
            PlayerPrefs.DeleteKey("LevelNumber");
        }
    }
}
