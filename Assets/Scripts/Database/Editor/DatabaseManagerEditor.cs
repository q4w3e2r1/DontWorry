using SQL_Quest.Components.UI;
using SQL_Quest.Database.Manager;
using SQL_Quest.Extentions;
using UnityEditor;

namespace SQL_Quest.Database.Editor
{
    [CustomEditor(typeof(DatabaseManager))]
    public class DatabaseManagerEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;

        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
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
                    case Mode.Level:
                        break;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}