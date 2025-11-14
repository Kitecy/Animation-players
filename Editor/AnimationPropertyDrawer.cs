using AnimationPlayer.Editor.Utils;
using AnimationPlayers.Players;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Animation = AnimationPlayers.Players.Animation;

namespace AnimationPlayers.Editor
{
    [CustomPropertyDrawer(typeof(Animation))]
    [CanEditMultipleObjects]
    public class AnimationPropertyDrawer : PropertyDrawer
    {
        private const string TypeError = "the type of animation you are trying to process is not supported!";
        private const string BasePlayerError = "This object does not support displaying objects in the inspector that do not inherit from BasePlayer.";

        private readonly int _errorFieldSCount = 2;
        private readonly int _simpleAnimationPlayerStandartFieldsCount = 10;

        private readonly int _standartFieldsCount = 12;

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

            if (property.serializedObject.targetObject is not SimpleAnimationPlayer && property.serializedObject.targetObject is not TriggeredAnimationPlayer)
            {
                if (GetAnimation(property) != null)
                {
                    Animation animation = GetAnimation(property);

                    animation.SetName(EditorGUI.TextField(FieldsCreator.GetLine(position, line), AnimationFieldsNames.NameFieldLabel, animation.Name));
                    line++;

                    animation.SetOrder(EditorGUI.IntField(FieldsCreator.GetLine(position, line), AnimationFieldsNames.OrderFieldLabel, animation.Order));
                    line++;
                }
                else
                {
                    FieldsCreator.DrawField(property, AnimationFieldsNames.NameField, position, ref line);
                    FieldsCreator.DrawField(property, AnimationFieldsNames.OrderField, position, ref line);
                }
            }

            DrawBaseFields(property, position, ref line);
            DrawTypeFields(property, position, ref line);

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool isEternalLoop;
            Animation.Type type;

            if (GetAnimation(property) != null)
            {
                Animation animation = GetAnimation(property);
                isEternalLoop = animation.IsEternalLoop;
                type = animation.Sort;
            }
            else
            {
                type = (Animation.Type)property.FindPropertyRelative(AnimationFieldsNames.TypeField).enumValueIndex;
                SerializedProperty isEternalLoopField = property.FindPropertyRelative(AnimationFieldsNames.IsEternalLoopField);
                isEternalLoop = isEternalLoopField.boolValue;
            }

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            int lines;

            if (property.serializedObject.targetObject is not BasePlayer)
            {
                lines = _errorFieldSCount;
                return height * lines;
            }

            if (property.isExpanded == false)
                return height;

            if (property.serializedObject.targetObject is SimpleAnimationPlayer || property.serializedObject.targetObject is TriggeredAnimationPlayer)
                lines = _simpleAnimationPlayerStandartFieldsCount;
            else
                lines = _standartFieldsCount;

            if (type == Animation.Type.Color || type == Animation.Type.Fade)
                lines++;

            if (isEternalLoop == false)
                lines++;

            return height * lines;
        }

        private void DrawBaseFields(SerializedProperty property, Rect position, ref int line)
        {
            if (GetAnimation(property) != null)
            {
                line = CreateBaseFieldsByManagedReferenceValue(property, position, line);
            }
            else
            {
                line = CreateBaseFieldsByRelative(property, position, ref line);
            }
        }

        private int CreateBaseFieldsByManagedReferenceValue(SerializedProperty property, Rect position, int line)
        {
            Animation animation = GetAnimation(property);

            animation.SetDuration(FieldsCreator.DrawFloatField(animation.Duration, AnimationFieldsNames.DurationFieldLabel, position, ref line));
            animation.SetDelay(FieldsCreator.DrawFloatField(animation.Delay, AnimationFieldsNames.DelayFieldLabel, position, ref line));

            GUI.enabled = false;
            FieldsCreator.DrawFloatField(animation.TotalDuration, AnimationFieldsNames.TotalDurationFieldLabel, position, ref line);
            GUI.enabled = true;

            animation.SetIsEternalLoop(EditorGUI.Toggle(FieldsCreator.GetLine(position, line), AnimationFieldsNames.IsEternalLoopFieldLabel, animation.IsEternalLoop));
            line++;

            if (animation.IsEternalLoop == false)
                animation.SetLoops(FieldsCreator.DrawIntFieldWithMin(0, animation.Loops, AnimationFieldsNames.LoopsFieldLabel, position, ref line));

            animation.SetLoopType((LoopType)EditorGUI.EnumFlagsField(FieldsCreator.GetLine(position, line), AnimationFieldsNames.LoopTypeFieldLabel, animation.LoopType));
            line++;

            animation.SetEase((Ease)EditorGUI.EnumFlagsField(FieldsCreator.GetLine(position, line), AnimationFieldsNames.EaseFieldLabel, animation.Ease));
            line++;

            animation.SetType((Animation.Type)EditorGUI.EnumFlagsField(FieldsCreator.GetLine(position, line), AnimationFieldsNames.TypeFieldLabel, animation.Sort));
            line++;

            return line;
        }

        private int CreateBaseFieldsByRelative(SerializedProperty property, Rect position, ref int line)
        {
            SerializedProperty durationField = property.FindPropertyRelative(AnimationFieldsNames.DurationField);
            SerializedProperty delayField = property.FindPropertyRelative(AnimationFieldsNames.DelayField);
            SerializedProperty isEternalLoopField = property.FindPropertyRelative(AnimationFieldsNames.IsEternalLoopField);
            SerializedProperty loopsField = property.FindPropertyRelative(AnimationFieldsNames.LoopsField);

            durationField.floatValue = FieldsCreator.DrawFloatFieldWithMin(Animation.MinDurationValue, durationField.floatValue, AnimationFieldsNames.DurationFieldLabel, position, ref line);
            delayField.floatValue = FieldsCreator.DrawFloatFieldWithMin(0, delayField.floatValue, AnimationFieldsNames.DelayFieldLabel, position, ref line);

            GUI.enabled = false;
            FieldsCreator.DrawFloatField(durationField.floatValue + delayField.floatValue, AnimationFieldsNames.TotalDurationFieldLabel, position, ref line);
            GUI.enabled = true;

            FieldsCreator.DrawField(property, AnimationFieldsNames.IsEternalLoopField, position, ref line);

            if (isEternalLoopField.boolValue == false)
            {
                loopsField.intValue = FieldsCreator.DrawIntFieldWithMin(0, loopsField.intValue, AnimationFieldsNames.LoopsFieldLabel, position, ref line);
            }
            else
            {
                loopsField.intValue = _eternalLoopValue;
            }

            FieldsCreator.DrawField(property, AnimationFieldsNames.LoopTypeField, position, ref line);

            FieldsCreator.DrawField(property, AnimationFieldsNames.EaseField, position, ref line);
            FieldsCreator.DrawField(property, AnimationFieldsNames.TypeField, position, ref line);

            return line;
        }

        private void DrawTypeFields(SerializedProperty property, Rect position, ref int line)
        {
            Animation.Type type;

            if (GetAnimation(property) != null)
            {
                Animation animation = GetAnimation(property);
                type = animation.Sort;
            }
            else
            {
                SerializedProperty typeField = property.FindPropertyRelative(AnimationFieldsNames.TypeField);
                type = (Animation.Type)typeField.enumValueIndex;
            }

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
            if (GetAnimation(property) != null)
            {
                Animation animation = GetAnimation(property);

                animation.SetStartPosition(EditorGUI.Vector3Field(FieldsCreator.GetLine(position, line), AnimationFieldsNames.StartPositionFieldLabel, animation.StartPosition));
                line++;

                animation.SetEndPosition(EditorGUI.Vector3Field(FieldsCreator.GetLine(position, line), AnimationFieldsNames.EndPositionFieldLabel, animation.EndPosition));
                line++;
            }
            else
            {
                FieldsCreator.DrawField(property, AnimationFieldsNames.StartPositionField, position, ref line);
                FieldsCreator.DrawField(property, AnimationFieldsNames.EndPositionField, position, ref line);
            }
        }

        private void DrawRotationFields(SerializedProperty property, Rect position, ref int line)
        {
            if (GetAnimation(property) != null)
            {
                Animation animation = GetAnimation(property);

                animation.SetStartRotation(EditorGUI.Vector3Field(FieldsCreator.GetLine(position, line), AnimationFieldsNames.StartRotationFieldLabel, animation.StartRotation));
                line++;

                animation.SetEndRotation(EditorGUI.Vector3Field(FieldsCreator.GetLine(position, line), AnimationFieldsNames.EndRotationFieldLabel, animation.EndRotation));
                line++;
            }
            else
            {
                FieldsCreator.DrawField(property, AnimationFieldsNames.StartRotationField, position, ref line);
                FieldsCreator.DrawField(property, AnimationFieldsNames.EndRotationField, position, ref line);
            }
        }

        private void DrawScaleFields(SerializedProperty property, Rect position, ref int line)
        {
            if (GetAnimation(property) != null)
            {
                Animation animation = GetAnimation(property);

                animation.SetStartScale(EditorGUI.Vector3Field(FieldsCreator.GetLine(position, line), AnimationFieldsNames.StartScaleFieldLabel, animation.StartScale));
                line++;

                animation.SetEndScale(EditorGUI.Vector3Field(FieldsCreator.GetLine(position, line), AnimationFieldsNames.EndScaleFieldLabel, animation.EndScale));
                line++;
            }
            else
            {
                FieldsCreator.DrawField(property, AnimationFieldsNames.StartScaleField, position, ref line);
                FieldsCreator.DrawField(property, AnimationFieldsNames.EndScaleField, position, ref line);
            }
        }

        private void DrawColorFields(SerializedProperty property, Rect position, ref int line)
        {
            if (GetAnimation(property) != null)
            {
                Animation animation = GetAnimation(property);

                DrawRenderField(property, position, ref line);

                animation.SetStartColor(EditorGUI.ColorField(FieldsCreator.GetLine(position, line), AnimationFieldsNames.StartColorFieldLabel, animation.StartColor));
                line++;

                animation.SetEndColor(EditorGUI.ColorField(FieldsCreator.GetLine(position, line), AnimationFieldsNames.EndColorFieldLabel, animation.EndColor));
                line++;
            }
            else
            {
                DrawRenderField(property, position, ref line);

                FieldsCreator.DrawField(property, AnimationFieldsNames.StartColorField, position, ref line);
                FieldsCreator.DrawField(property, AnimationFieldsNames.EndColorField, position, ref line);
            }
        }

        private void DrawFadeFields(SerializedProperty property, Rect position, ref int line)
        {
            if (GetAnimation(property) != null)
            {
                Animation animation = GetAnimation(property);

                DrawRenderField(property, position, ref line);

                animation.SetStartFade(FieldsCreator.DrawFloatField(animation.StartFade, AnimationFieldsNames.StartFadeFieldLabel, position, ref line));
                animation.SetEndFade(FieldsCreator.DrawFloatField(animation.EndFade, AnimationFieldsNames.EndFadeFieldLabel, position, ref line));
            }
            else
            {
                DrawRenderField(property, position, ref line);

                SerializedProperty startFadeProperty = property.FindPropertyRelative(AnimationFieldsNames.StartFadeField);
                SerializedProperty endFadeProperty = property.FindPropertyRelative(AnimationFieldsNames.EndFadeField);

                startFadeProperty.floatValue = FieldsCreator.DrawFloatFieldWithMin(0, startFadeProperty.floatValue, AnimationFieldsNames.StartFadeField, position, ref line);
                endFadeProperty.floatValue = FieldsCreator.DrawFloatFieldWithMin(0, endFadeProperty.floatValue, AnimationFieldsNames.EndFadeField, position, ref line);
            }
        }

        private void DrawRenderField(SerializedProperty property, Rect position, ref int line)
        {
            if (GetAnimation(property) != null)
            {
                Animation animation = GetAnimation(property);

                if ((property.serializedObject.targetObject as BasePlayer).IsUsingInUI)
                    animation.SetGraphic((Graphic)EditorGUI.ObjectField(FieldsCreator.GetLine(position, line), AnimationFieldsNames.GraphicFieldLabel, animation.Graphic, typeof(Graphic), true));
                else
                    animation.SetRenderer((Renderer)EditorGUI.ObjectField(FieldsCreator.GetLine(position, line), AnimationFieldsNames.RendererFieldLabel, animation.Renderer, typeof(Renderer), true));

                line++;
            }
            else
            {
                if ((property.serializedObject.targetObject as BasePlayer).IsUsingInUI)
                    FieldsCreator.DrawField(property, AnimationFieldsNames.GraphicField, position, ref line);
                else
                    FieldsCreator.DrawField(property, AnimationFieldsNames.RendererField, position, ref line);
            }
        }

        private void DrawAnchorFields(SerializedProperty property, Rect position, ref int line)
        {
            if (GetAnimation(property) != null)
            {
                Animation animation = GetAnimation(property);

                animation.SetStartAnchoredPosition(EditorGUI.Vector3Field(FieldsCreator.GetLine(position, line), AnimationFieldsNames.StartAnchorFieldLabel, animation.StartAnchorPosition));
                line++;

                animation.SetEndAnchoredPosition(EditorGUI.Vector3Field(FieldsCreator.GetLine(position, line), AnimationFieldsNames.StartAnchorFieldLabel, animation.EndAnchorPosition));
                line++;
            }
            else
            {
                FieldsCreator.DrawField(property, AnimationFieldsNames.StartAnchorField, position, ref line);
                FieldsCreator.DrawField(property, AnimationFieldsNames.EndAnchorField, position, ref line);
            }
        }

        private Animation GetAnimation(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference)
                return null;

            return property.managedReferenceValue as Animation;
        }
    }
}
