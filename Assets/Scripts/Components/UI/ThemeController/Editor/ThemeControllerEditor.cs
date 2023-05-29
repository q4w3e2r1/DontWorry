using SQL_Quest.Extentions;
using UnityEditor;

namespace SQL_Quest.Components.UI.ThemeController.Editor
{
    [CustomEditor(typeof(ThemeController))]
    public class ThemeControllerEditor : UnityEditor.Editor
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
                    case Mode.Sprite:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_darkThemeSprite"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_lightThemeSprite"));
                        break;
                    case Mode.Color:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_darkThemeColor"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_lightThemeColor"));
                        break;
                    case Mode.Both:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_darkThemeSprite"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_lightThemeSprite"));

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_darkThemeColor"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_lightThemeColor"));
                        break;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}