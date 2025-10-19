using AnimationPlayer.Editor.Utils;
using System;
using UnityEditor;
using UnityEngine;
using Animation = AnimationPlayers.Players.Animation;

namespace AnimationPlayers.Editor
{
    public abstract class BaseEditor : UnityEditor.Editor
    {
        [NonSerialized] protected SerializedProperty DrawableAnimation = null;

        protected string AnimationRecordName = "Animation Edit";

        protected Vector3 LabelOffset = new(0, 0.4f, 0);
        protected int LabelFontSize = 30;

        protected string StartPositionHandleName = "Start";
        protected Color StartPositionHandleColor = Color.green;

        protected string EndPositionHandleName = "End";
        protected Color EndPositionHandleColor = Color.red;

        private readonly string _isUiFieldName = "IsUI";
        private readonly string _autoCallFieldName = "_autoCall";

        protected void OnSceneGUI()
        {
            if (DrawableAnimation != null)
            {
                SerializedProperty typeField = DrawableAnimation.FindPropertyRelative(AnimationFieldsNames.TypeField);

                if ((Animation.Type)typeField.intValue == Animation.Type.Position)
                    DrawHandles();
            }
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty isUIField = serializedObject.FindProperty(_isUiFieldName);
            SerializedProperty autoCallField = serializedObject.FindProperty(_autoCallFieldName);

            EditorGUILayout.PropertyField(isUIField);
            EditorGUILayout.PropertyField(autoCallField);
        }

        protected virtual void DrawHandles()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            SerializedProperty startPositionField = DrawableAnimation.FindPropertyRelative(AnimationFieldsNames.StartPositionField);
            SerializedProperty endPositionField = DrawableAnimation.FindPropertyRelative(AnimationFieldsNames.EndPositionField);

            DrawLabel(startPositionField.vector3Value + LabelOffset, StartPositionHandleName, StartPositionHandleColor);
            DrawLabel(endPositionField.vector3Value + LabelOffset, EndPositionHandleName, EndPositionHandleColor);

            Vector3 endPosition = Handles.PositionHandle(endPositionField.vector3Value, Quaternion.identity);
            Vector3 startPosition = Handles.PositionHandle(startPositionField.vector3Value, Quaternion.identity);
            Handles.DrawLine(startPosition, endPosition);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(serializedObject.targetObject, AnimationRecordName);
                startPositionField.vector3Value = startPosition;
                endPositionField.vector3Value = endPosition;
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawLabel(Vector3 position, string label, Color color)
        {
            GUIStyle labelStyle = new();
            labelStyle.fontSize = LabelFontSize;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = color;

            Handles.Label(position, label, labelStyle);
        }
    }
}
