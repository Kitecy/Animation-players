using AnimationPlayer.Editor.Utils;
using AnimationPlayers.Players;
using UnityEditor;
using UnityEngine;
using Animation = AnimationPlayers.Players.Animation;

namespace AnimationPlayers.Editor
{
    [CustomPropertyDrawer(typeof(Animation))]
    public class AnimationPropertyDrawer : PropertyDrawer
    {
        private const string TypeError = "the type of animation you are trying to process is not supported!";
        private const string BasePlayerError = "This object does not support displaying objects in the inspector that do not inherit from BasePlayer.";

        private const int MinValueForTimeFields = 0;

        private readonly string _durationFieldLabel = "Duration";
        private readonly string _delayFieldLabel = "Delay";
        private readonly string _totalDurationFieldLabel = "Total Duration";
        private readonly string _loopsFieldLabel = "Loops";

        private readonly int _errorFieldSCount = 2;
        private readonly int _simpleAnimationPlayerStandartFieldsCount = 9;

        private readonly int _standartFieldsCount = 11;

        private readonly int _eternalLoopValue = -1;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int line = 0;

            if (property.serializedObject.targetObject is not BasePlayer)
            {
                EditorGUI.HelpBox(position, BasePlayerError, MessageType.Error);
                return;
            }

            property.isExpanded = FieldsCreator.DrawFoldout(position, property.isExpanded, label, ref line);

            if (property.isExpanded == false)
                return;

            EditorGUI.indentLevel++;

            if (property.serializedObject.targetObject is not SimpleAnimationPlayer)
            {
                FieldsCreator.DrawField(property, AnimationFieldsNames.NameField, position, ref line);
                FieldsCreator.DrawField(property, AnimationFieldsNames.OrderField, position, ref line);
            }

            DrawBaseFields(property, position, ref line);
            DrawTypeFields(property, position, ref line);

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Animation.Type type = (Animation.Type)property.FindPropertyRelative(AnimationFieldsNames.TypeField).enumValueIndex;
            SerializedProperty isEternalLoopField = property.FindPropertyRelative(AnimationFieldsNames.IsEternalLoopField);

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            int lines;

            if (property.serializedObject.targetObject is not BasePlayer)
            {
                lines = _errorFieldSCount;
                return height * lines;
            }

            if (property.isExpanded == false)
                return height;

            if (property.serializedObject.targetObject is SimpleAnimationPlayer)
                lines = _simpleAnimationPlayerStandartFieldsCount;
            else
                lines = _standartFieldsCount;

            if (type == Animation.Type.Color || type == Animation.Type.Fade)
                lines++;

            if (isEternalLoopField.boolValue == false)
                lines++;

            return height * lines;
        }

        private void DrawBaseFields(SerializedProperty property, Rect position, ref int line)
        {
            SerializedProperty durationField = property.FindPropertyRelative(AnimationFieldsNames.DurationField);
            SerializedProperty delayField = property.FindPropertyRelative(AnimationFieldsNames.DelayField);
            SerializedProperty isEternalLoopField = property.FindPropertyRelative(AnimationFieldsNames.IsEternalLoopField);
            SerializedProperty loopsField = property.FindPropertyRelative(AnimationFieldsNames.LoopsField);

            durationField.floatValue = FieldsCreator.DrawFloatFieldWithMin(MinValueForTimeFields, durationField.floatValue, _durationFieldLabel, position, ref line);
            delayField.floatValue = FieldsCreator.DrawFloatFieldWithMin(MinValueForTimeFields, delayField.floatValue, _delayFieldLabel, position, ref line);

            GUI.enabled = false;
            FieldsCreator.DrawFloatField(durationField.floatValue + delayField.floatValue, _totalDurationFieldLabel, position, ref line);
            GUI.enabled = true;

            FieldsCreator.DrawField(property, AnimationFieldsNames.IsEternalLoopField, position, ref line);

            if (isEternalLoopField.boolValue == false)
            {
                loopsField.intValue = FieldsCreator.DrawIntFieldWithMin(MinValueForTimeFields, loopsField.intValue, _loopsFieldLabel, position, ref line);
            }
            else
            {
                loopsField.intValue = _eternalLoopValue;
            }

            FieldsCreator.DrawField(property, AnimationFieldsNames.LoopTypeField, position, ref line);

            FieldsCreator.DrawField(property, AnimationFieldsNames.TypeField, position, ref line);
        }

        private void DrawTypeFields(SerializedProperty property, Rect position, ref int line)
        {
            SerializedProperty typeField = property.FindPropertyRelative(AnimationFieldsNames.TypeField);

            Animation.Type type = (Animation.Type)typeField.enumValueIndex;

            switch (type)
            {
                case Animation.Type.Position:
                    DrawPositionFields(property, position, ref line);
                    break;

                case Animation.Type.Rotation:
                    DrawRotationFields(property, position, ref line);
                    break;

                case Animation.Type.Scale:
                    DrawScaleFields(property, position, ref line);
                    break;

                case Animation.Type.Color:
                    DrawColorFields(property, position, ref line);
                    break;

                case Animation.Type.Fade:
                    DrawFadeFields(property, position, ref line);
                    break;

                case Animation.Type.Anchor:
                    DrawAnchorFields(property, position, ref line);
                    break;

                default:
                    throw new System.InvalidOperationException(TypeError);
            }
        }

        private void DrawPositionFields(SerializedProperty property, Rect position, ref int line)
        {
            FieldsCreator.DrawField(property, AnimationFieldsNames.StartPositionField, position, ref line);
            FieldsCreator.DrawField(property, AnimationFieldsNames.EndPositionField, position, ref line);
        }

        private void DrawRotationFields(SerializedProperty property, Rect position, ref int line)
        {
            FieldsCreator.DrawField(property, AnimationFieldsNames.StartRotationField, position, ref line);
            FieldsCreator.DrawField(property, AnimationFieldsNames.EndRotationField, position, ref line);
        }

        private void DrawScaleFields(SerializedProperty property, Rect position, ref int line)
        {
            FieldsCreator.DrawField(property, AnimationFieldsNames.StartScaleField, position, ref line);
            FieldsCreator.DrawField(property, AnimationFieldsNames.EndScaleField, position, ref line);
        }

        private void DrawColorFields(SerializedProperty property, Rect position, ref int line)
        {
            DrawRenderField(property, position, ref line);

            FieldsCreator.DrawField(property, AnimationFieldsNames.StartColorField, position, ref line);
            FieldsCreator.DrawField(property, AnimationFieldsNames.EndColorField, position, ref line);
        }

        private void DrawFadeFields(SerializedProperty property, Rect position, ref int line)
        {
            DrawRenderField(property, position, ref line);

            FieldsCreator.DrawField(property, AnimationFieldsNames.StartFadeField, position, ref line);
            FieldsCreator.DrawField(property, AnimationFieldsNames.EndFadeField, position, ref line);
        }

        private void DrawRenderField(SerializedProperty property, Rect position, ref int line)
        {
            if ((property.serializedObject.targetObject as BasePlayer).IsUsingInUI)
                FieldsCreator.DrawField(property, AnimationFieldsNames.GraphicField, position, ref line);
            else
                FieldsCreator.DrawField(property, AnimationFieldsNames.RendererField, position, ref line);
        }

        private void DrawAnchorFields(SerializedProperty property, Rect position, ref int line)
        {
            FieldsCreator.DrawField(property, AnimationFieldsNames.StartAnchorField, position, ref line);
            FieldsCreator.DrawField(property, AnimationFieldsNames.EndAnchorField, position, ref line);
        }
    }
}
