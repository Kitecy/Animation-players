
namespace AnimationPlayers
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(AdditiveAnimation))]
    public class AdditiveAnimationPropetyDrawer : PropertyDrawer
    {
        private readonly int _linesCount = 6;
        private readonly int _linesCountWhenColorMode = 7;

        private readonly string _additivePositionFieldName = "_additivePosition";
        private readonly string _additiveScaleFieldName = "_additiveScale";
        private readonly string _additiveRotationFieldName = "_additiveRotation";
        private readonly string _additiveColorFieldName = "_additiveColor";

        private readonly string _durationFieldLabel = "Duration";
        private readonly string _durationFieldName = "_duration";

        private readonly string _delayFieldLabel = "Delay";
        private readonly string _delayFieldName = "_delay";

        private readonly string _spriteRendererFieldName = "_spriteRenderer";
        private readonly string _rendererFieldName = "_renderer";
        private readonly string _graphicFieldName = "_graphic";

        private readonly string _totalDurationLabel = "Total Duration";

        private readonly string _onEnableFieldName = "_onEnable";
        private readonly string _typeFieldName = "_type";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int line = 0;

            property.isExpanded = EditorGUI.Foldout(GetLine(position, line), property.isExpanded, label);
            line++;

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                DrawFieldWithMinValue(position, property, _durationFieldName, _durationFieldLabel, 0, ref line);
                DrawFieldWithMinValue(position, property, _delayFieldName, _delayFieldLabel, 0, ref line);
                DrawTotalDurationField(position, property, ref line);

                DrawField(position, property, _onEnableFieldName, ref line);
                DrawField(position, property, _typeFieldName, ref line);
                DrawTypeFields(position, property, ref line);

                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                SerializedProperty typeField = property.FindPropertyRelative(_typeFieldName);
                AdditiveAnimation.AdditiveAnimationType type = (AdditiveAnimation.AdditiveAnimationType)typeField.enumValueIndex;

                if (type == AdditiveAnimation.AdditiveAnimationType.Color)
                    height += _linesCountWhenColorMode * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                else
                    height += _linesCount * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            }

            return height;
        }

        private void DrawField(Rect position, SerializedProperty property, string fieldName, ref int line)
        {
            SerializedProperty field = property.FindPropertyRelative(fieldName);
            EditorGUI.PropertyField(GetLine(position, line), field);
            line++;
        }

        private void DrawFieldWithMinValue(Rect position, SerializedProperty property, string fieldName, string fieldLabel, float min, ref int line)
        {
            SerializedProperty field = property.FindPropertyRelative(fieldName);
            field.floatValue = Mathf.Max(min, EditorGUI.FloatField(GetLine(position, line), fieldLabel, field.floatValue));
            line++;
        }

        private void DrawTotalDurationField(Rect position, SerializedProperty property, ref int line)
        {
            SerializedProperty durationField = property.FindPropertyRelative(_durationFieldName);
            SerializedProperty delayField = property.FindPropertyRelative(_delayFieldName);

            GUI.enabled = false;
            EditorGUI.FloatField(GetLine(position, line), _totalDurationLabel, durationField.floatValue + delayField.floatValue);
            GUI.enabled = true;

            line++;
        }

        private void DrawTypeFields(Rect position, SerializedProperty property, ref int line)
        {
            SerializedProperty typeField = property.FindPropertyRelative(_typeFieldName);

            AdditiveAnimation.AdditiveAnimationType type = (AdditiveAnimation.AdditiveAnimationType)typeField.enumValueIndex;

            switch (type)
            {
                case AdditiveAnimation.AdditiveAnimationType.Position:
                    DrawField(position, property, _additivePositionFieldName, ref line);
                    break;

                case AdditiveAnimation.AdditiveAnimationType.Scale:
                    DrawField(position, property, _additiveScaleFieldName, ref line);
                    break;

                case AdditiveAnimation.AdditiveAnimationType.Rotation:
                    DrawField(position, property, _additiveRotationFieldName, ref line);
                    break;

                case AdditiveAnimation.AdditiveAnimationType.Color:
                    DrawColorFields(position, property, type, ref line);
                    break;
            }
        }

        private void DrawColorFields(Rect position, SerializedProperty property, AdditiveAnimation.AdditiveAnimationType type, ref int line)
        {
            AdditiveAnimationPlayer player = property.serializedObject.targetObject as AdditiveAnimationPlayer;

            switch (player)
            {
                case AdditiveAnimationPlayer2D:
                    DrawField(position, property, _spriteRendererFieldName, ref line);
                    break;

                case AdditiveAnimationPlayer3D:
                    DrawField(position, property, _rendererFieldName, ref line);
                    break;

                case AdditiveAnimationPlayerUI:
                    DrawField(position, property, _graphicFieldName, ref line);
                    break;
            }

            DrawField(position, property, _additiveColorFieldName, ref line);
        }

        private Rect GetLine(Rect baseRect, int line, int lineHeightMultiplier = 1)
        {
            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float standardVerticalSpacing = EditorGUIUtility.standardVerticalSpacing;

            return new Rect(baseRect.x, baseRect.y + line * (singleLineHeight + standardVerticalSpacing), baseRect.width, singleLineHeight * lineHeightMultiplier);
        }
    }
}
