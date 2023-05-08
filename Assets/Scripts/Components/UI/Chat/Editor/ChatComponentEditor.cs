using SQL_Quest.Extentions;
using UnityEditor;

namespace SQL_Quest.Components.UI.Chat.Editor
{
    [CustomEditor(typeof(ChatComponent))]
    public class ChatComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _helpButtonProperty;
        private SerializedProperty _completeButtonProperty;
        private SerializedProperty _restartButtonProperty;

        private SerializedProperty _messagePrefabProperty;

        private SerializedProperty _modeProperty;

        private void OnEnable()
        {
            _helpButtonProperty = serializedObject.FindProperty("_helpButton");
            _completeButtonProperty = serializedObject.FindProperty("_completeButton");
            _restartButtonProperty = serializedObject.FindProperty("_restartButton");

            _messagePrefabProperty = serializedObject.FindProperty("_messagePrefab");

            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_helpButtonProperty);
            EditorGUILayout.PropertyField(_completeButtonProperty);
            EditorGUILayout.PropertyField(_restartButtonProperty);

            EditorGUILayout.PropertyField(_messagePrefabProperty);

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