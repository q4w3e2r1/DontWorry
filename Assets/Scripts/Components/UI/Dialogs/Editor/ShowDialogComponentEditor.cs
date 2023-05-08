using SQL_Quest.Extentions;
using UnityEditor;

namespace SQL_Quest.Components.UI.Dialogs.Editor
{
    [CustomEditor(typeof(ShowDialogComponent))]
    public class ShowDialogComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _onStartProperty;
        private SerializedProperty _onFinishProperty;

        private SerializedProperty _modeProperty;

        private void OnEnable()
        {
            _onStartProperty = serializedObject.FindProperty("_onStart");
            _onFinishProperty = serializedObject.FindProperty("_onFinish");

            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_onStartProperty);
            EditorGUILayout.PropertyField(_onFinishProperty);

            EditorGUILayout.PropertyField(_modeProperty);

            if (_modeProperty.GetEnum(out Mode mode))
            {
                switch (mode)
                {
                    case Mode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bound"));
                        break;
                    case Mode.External:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_external"));
                        break;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}