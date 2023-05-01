using UnityEngine;

namespace SQL_Quest.Components.UI.Dialogs
{
    [CreateAssetMenu(fileName = "Dialog")]
    public class DialogDef : ScriptableObject
    {
        [SerializeField] private DialogData _data;
        public DialogData Data => _data;
    }
}