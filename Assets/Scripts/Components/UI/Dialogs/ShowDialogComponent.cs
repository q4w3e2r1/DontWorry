using SQL_Quest.Creatures.Player;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SQL_Quest.Components.UI.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onStart;
        [SerializeField] private UnityEvent _onFinish;
        [Space]
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;

        private DialogBoxController _dialogBox;

        private DialogData _data
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
                        var playerData = PlayerDataHandler.PlayerData;
                        return Resources.Load<DialogDef>($"Levels/Level{playerData.LevelNumber}/Dialogs/{gameObject.name}").Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void Show()
        {
            _dialogBox = FindDialogController();
            _dialogBox.ShowDialog(_data, _onStart, _onFinish);
        }

        private DialogBoxController FindDialogController()
        {
            if (_dialogBox != null)
                return _dialogBox;

            /*GameObject controllerGO = _data.Type switch
            {
                DialogType.Simple => GameObject.FindWithTag("SimpleDialog"),
                DialogType.Personalized => GameObject.FindWithTag("PersonalizedDialog"),
                _ => throw new ArgumentException("Undefined dialog type"),
            };*/

            var controllerGO = GameObject.FindWithTag("SimpleDialog");
            return controllerGO.GetComponent<DialogBoxController>();
        }
    }
}