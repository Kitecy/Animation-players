using AnimationPlayers.Players;
using System;
using UnityEditor;
using UnityEngine;
using Animation = AnimationPlayers.Players.Animation;

namespace AnimationPlayers.Editor
{
    public abstract class BaseEditor : UnityEditor.Editor
    {
        protected const string MultipleEditingError = "Changing this parameter is not supported in multi edit mode.";

        [NonSerialized] protected IReadOnlyAnimation DrawableAnimation = null;

        protected string AnimationRecordName = "Animation Edit";

        protected Vector3 LabelOffset = new(0, 0.4f, 0);
        protected int LabelFontSize = 30;

        protected string StartPositionHandleName = "Start";
        protected Color StartPositionHandleColor = Color.green;

        protected string EndPositionHandleName = "End";
        protected Color EndPositionHandleColor = Color.red;

        private readonly string _isUiFieldName = "IsUI";
        private readonly string _autoCallFieldName = "_autoCall";

        private SerializedProperty _isUIField;
        private SerializedProperty _autoCallField;

        protected virtual void OnEnable()
        {
            _isUIField = serializedObject.FindProperty(_isUiFieldName);
            _autoCallField = serializedObject.FindProperty(_autoCallFieldName);
        }

        protected void OnSceneGUI()
        {
            if (DrawableAnimation == null)
                return;

            if (DrawableAnimation.Sort == Animation.Type.Position)
                DrawHandles();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_isUIField);
            EditorGUILayout.PropertyField(_autoCallField);
        }

        protected virtual void DrawHandles()
        {
            EditorGUI.BeginChangeCheck();

            Animation animation = DrawableAnimation as Animation;

            Vector3 startPositionField = animation.StartPosition;
            Vector3 endPositionField = animation.EndPosition;

            DrawLabel(startPositionField + LabelOffset, StartPositionHandleName, StartPositionHandleColor);
            DrawLabel(endPositionField + LabelOffset, EndPositionHandleName, EndPositionHandleColor);

            Vector3 endPosition = Handles.PositionHandle(endPositionField, Quaternion.identity);
            Vector3 startPosition = Handles.PositionHandle(startPositionField, Quaternion.identity);
            Handles.DrawLine(startPosition, endPosition);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, AnimationRecordName);
                animation.SetStartPosition(startPosition);
                animation.SetEndPosition(endPosition);
            }
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
