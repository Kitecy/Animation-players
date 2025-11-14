using UnityEditor;
using UnityEngine;

namespace AnimationPlayer.Editor.Utils
{
    public static class FieldsCreator
    {
        public static void DrawField(SerializedProperty property, string fieldName, Rect position, ref int line)
        {
            SerializedProperty field = property.FindPropertyRelative(fieldName);

            EditorGUI.PropertyField(GetLine(position, line), field);
            line++;
        }

        public static float DrawFloatField(float value, string label, Rect position, ref int line)
        {
            value = EditorGUI.FloatField(GetLine(position, line), label, value);
            line++;
            return value;
        }

        public static float DrawFloatFieldWithMin(float min, float value, string label, Rect position, ref int line)
        {
            value = EditorGUI.FloatField(GetLine(position, line), label, Mathf.Max(min, value));
            line++;
            return value;
        }

        public static int DrawIntFieldWithMin(int min, int value, string label, Rect position, ref int line)
        {
            value = EditorGUI.IntField(GetLine(position, line), label, Mathf.Max(min, value));
            line++;
            return value;
        }

        public static bool DrawFoldout(Rect position, bool foldout, GUIContent label, ref int line)
        {
            bool isExpanded = EditorGUI.Foldout(GetLine(position, line), foldout, label, true);
            line++;

            return isExpanded;
        }

        public static Rect GetLine(Rect baseRect, int line, int lineHeightMultiplier = 1)
        {
            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float standardVerticalSpacing = EditorGUIUtility.standardVerticalSpacing;

            return new Rect(baseRect.x, baseRect.y + line * (singleLineHeight + standardVerticalSpacing), baseRect.width, singleLineHeight * lineHeightMultiplier);
        }
    }
}
