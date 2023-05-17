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
                return _mode switch
                {
                    Mode.Bound => _bound,
                    Mode.External => _external.Data,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
        }

        public void Show()
        {
            _dialogBox = FindDialogController();
            _dialogBox.ShowDialog(_data, _onStart, _onFinish);
        }

        public void Show(DialogDef data)
        {
            _dialogBox = FindDialogController();
            _dialogBox.ShowDialog(data.Data, _onStart, _onFinish);
        }

        private DialogBoxController FindDialogController()
        {
            if (_dialogBox != null)
                return _dialogBox;
            GameObject controllerGO = _data.Type switch
            {
                DialogType.Simple => GameObject.FindWithTag("SimpleDialog"),
                DialogType.Personalized => GameObject.FindWithTag("PersonalizedDialog"),
                _ => throw new ArgumentException("Undefined dialog type"),
            };
            return controllerGO.GetComponent<DialogBoxController>();
        }
    }
}