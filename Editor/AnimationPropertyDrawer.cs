namespace AnimationPlayers
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(Animation))]
    public class AnimationPropertyDrawer : PropertyDrawer
    {
        private const int LinesCountWhenColorMode = 12;
        private const int LinesCount = 11;

        private const int CropedLinesCountWhenColorMode = 10;
        private const int CropedLinesCount = 9;

        private string _nameOfNameField = "_name";
        private string _orderFieldName = "_order";
        private string _typeFieldName = "_type";
        private string _playOnEnableFieldName = "_playOnEnable";
        private string _durationFieldName = "_duration";

        private string _defayFieldLabel = "Delay";
        private string _delayFieldName = "_delay";

        private string _startPositionFieldName = "_startPosition";
        private string _endPositionFieldName = "_endPosition";

        private string _startScaleFieldName = "_startScale";
        private string _endScaleFieldName = "_endScale";

        private string _startRotationFieldName = "_startRotation";
        private string _endRotationFieldName = "_endRotation";

        private string _rendererFieldName = "_renderer";
        private string _spriteRendererFieldName = "_spriteRenderer";
        private string _graphicFieldName = "_graphic";

        private string _startColorFieldName = "_startColor";
        private string _endColorFieldName = "_endColor";

        private string _easeFieldName = "_ease";

        private string _totalDurationHeader = "Total Duration";

        private SerializedProperty _typeProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();
            int line = 0;
            _typeProperty = property.FindPropertyRelative(_typeFieldName);

            EditorGUI.BeginProperty(position, label, property);

            property.isExpanded = EditorGUI.Foldout(GetLine(position, line), property.isExpanded, label);
            line++;

            if (property.isExpanded == false)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;

            if (property.serializedObject.targetObject is not SimpleAnimationPlayer)
            {
                DrawField(property, _nameOfNameField, position, ref line);
                DrawField(property, _orderFieldName, position, ref line);
            }

            DrawField(property, _durationFieldName, position, ref line);
            DrawDelayField(property, _defayFieldLabel, position, ref line);

            GUI.enabled = false;
            float totalDuration = property.FindPropertyRelative(_durationFieldName).floatValue + property.FindPropertyRelative(_delayFieldName).floatValue;
            EditorGUI.FloatField(GetLine(position, line), _totalDurationHeader, totalDuration);
            line++;
            GUI.enabled = true;

            DrawField(property, _typeFieldName, position, ref line);
            DrawField(property, _playOnEnableFieldName, position, ref line);

            DrawTypeFields((Animation.AnimationType)_typeProperty.enumValueIndex, property, position, ref line);

            DrawField(property, _easeFieldName, position, ref line);

            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();

            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded == false)
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            _typeProperty = property.FindPropertyRelative(_typeFieldName);

            float height;

            if (property.serializedObject.targetObject is not SimpleAnimationPlayer)
            {
                if ((Animation.AnimationType)_typeProperty.enumValueIndex == Animation.AnimationType.Color)
                    height = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * LinesCountWhenColorMode;
                else
                    height = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * LinesCount;
            }
            else
            {
                if ((Animation.AnimationType)_typeProperty.enumValueIndex == Animation.AnimationType.Color)
                    height = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * CropedLinesCountWhenColorMode;
                else
                    height = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * CropedLinesCount;
            }

            return height;
        }

        private void DrawField(SerializedProperty property, string fieldName, Rect position, ref int line)
        {
            SerializedProperty field = property.FindPropertyRelative(fieldName);

            EditorGUI.PropertyField(GetLine(position, line), field);
            line++;
        }

        private void DrawDelayField(SerializedProperty property, string label, Rect position, ref int line)
        {
            SerializedProperty delayProperty = property.FindPropertyRelative(_delayFieldName);
            delayProperty.floatValue = Mathf.Max(0, EditorGUI.FloatField(GetLine(position, line), label, delayProperty.floatValue));
            line++;
        }

        private Rect GetLine(Rect baseRect, int line)
        {
            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float standardVerticalSpacing = EditorGUIUtility.standardVerticalSpacing;

            return new Rect(baseRect.x, baseRect.y + line * (singleLineHeight + standardVerticalSpacing), baseRect.width, singleLineHeight);
        }

        private void DrawTypeFields(Animation.AnimationType type, SerializedProperty property, Rect position, ref int line)
        {
            switch (type)
            {
                case Animation.AnimationType.Position:
                    DrawPositionFields(property, position, ref line);
                    break;

                case Animation.AnimationType.Scale:
                    DrawScaleFields(property, position, ref line);
                    break;

                case Animation.AnimationType.Rotation:
                    DrawRotationFields(property, position, ref line);
                    break;

                case Animation.AnimationType.Color:
                    DrawColorFields(property, position, ref line);
                    break;
            }
        }

        private void DrawPositionFields(SerializedProperty property, Rect position, ref int line)
        {
            DrawField(property, _startPositionFieldName, position, ref line);
            DrawField(property, _endPositionFieldName, position, ref line);
        }

        private void DrawScaleFields(SerializedProperty property, Rect position, ref int line)
        {
            DrawField(property, _startScaleFieldName, position, ref line);
            DrawField(property, _endScaleFieldName, position, ref line);
        }

        private void DrawRotationFields(SerializedProperty property, Rect position, ref int line)
        {
            DrawField(property, _startRotationFieldName, position, ref line);
            DrawField(property, _endRotationFieldName, position, ref line);
        }

        private void DrawColorFields(SerializedProperty property, Rect position, ref int line)
        {
            switch (property.serializedObject.targetObject)
            {
                case Simple2DAnimationPlayer:
                    DrawField(property, _spriteRendererFieldName, position, ref line);
                    break;

                case SimpleUIAnimationPlayer:
                    DrawField(property, _graphicFieldName, position, ref line);
                    break;

                case Simple3DAnimationPlayer:
                    DrawField(property, _rendererFieldName, position, ref line);
                    break;
            }

            DrawField(property, _startColorFieldName, position, ref line);
            DrawField(property, _endColorFieldName, position, ref line);
        }
    }

}
