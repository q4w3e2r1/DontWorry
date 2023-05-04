using System;
using UnityEngine;

namespace SQL_Quest.Components.UI.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;

        private DialogBoxController _dialogBox;

        public void Show()
        {
            _dialogBox = FindDialogController();
            _dialogBox.ShowDialog(Data);
        }

        private DialogBoxController FindDialogController()
        {
            if (_dialogBox != null)
                return _dialogBox;

            GameObject controllerGO;
            switch (Data.Type)
            {
                case DialogType.Simple:
                    controllerGO = GameObject.FindWithTag("SimpleDialog");
                    break;
                case DialogType.Personalized:
                    controllerGO = GameObject.FindWithTag("PersonalizedDialog");
                    break;
                default:
                    throw new ArgumentException("Undefined dialog type");
            }

            return controllerGO.GetComponent<DialogBoxController>();
        }

        public DialogData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public enum Mode
        {
            Bound,
            External
        }
    }
}