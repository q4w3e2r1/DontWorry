using SQL_Quest.Components.LevelManagement.CharactersController;
using SQL_Quest.Extentions;
using UnityEditor;

namespace SQL_Quest.Components.UI.Chat.Editor
{
    [CustomEditor(typeof(CharactersController))]
    public class CharactersControllerEditor : UnityEditor.Editor
    {
        private SerializedProperty _mrNormattProperty;
        private SerializedProperty _hilpyProperty;


        private SerializedProperty _modeProperty;

        private void OnEnable()
        {
            _mrNormattProperty = serializedObject.FindProperty("_mrNormatt");
            _hilpyProperty = serializedObject.FindProperty("_hilpy");

            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_mrNormattProperty);
            EditorGUILayout.PropertyField(_hilpyProperty);

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