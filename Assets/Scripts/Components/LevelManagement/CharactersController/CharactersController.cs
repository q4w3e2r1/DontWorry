using SQL_Quest.Components.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SQL_Quest.Components.LevelManagement.CharactersController
{
    public class CharactersController : MonoBehaviour
    {
        [SerializeField] private GameObject _mrNormatt;
        [SerializeField] private GameObject _hilpy;

        [SerializeField] private Mode _mode;
        [SerializeField] private CharactersControllerData _bound;
        [SerializeField] private CharactersControllerDef _external;

        private CharactersControllerData _data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    case Mode.Level:
                        var levelNumber = PlayerPrefs.GetInt("LevelNumber");
                        Debug.Log($"Levels/Level{levelNumber}/CharactersControllers/" +
                            $"{SceneManager.GetActiveScene().name}CharactersController");
                        return Resources.Load<CharactersControllerDef>
                            ($"Levels/Level{levelNumber}/CharactersControllers/" +
                            $"{SceneManager.GetActiveScene().name}CharactersController").Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Awake()
        {
            if (_data.HaveMrNormatt)
            {
                _mrNormatt.SetActive(true);
                _mrNormatt.transform.position = _data.MrNormattPos;
            }
            if (_data.HaveHilpy)
            {
                _hilpy.SetActive(true);
                _hilpy.transform.position = _data.HilpyPos;
            }
        }
    }
}
