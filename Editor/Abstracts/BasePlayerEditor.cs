namespace AnimationPlayers
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public class BasePlayerEditor : Editor
    {
        [NonSerialized] protected Animation DrawableAnimation = null;

        private string _animationRecordName = "Animation Edit";

        private Vector3 _labelOffset = new(0, 0.4f, 0);
        private int _labelFontSize = 30;

        private string _startPositionHandleName = "Start";
        private Color _startPositionHandleColor = Color.green;

        private string _endPositionHandleName = "End";
        private Color _endPositionHandleColor = Color.red;

        protected void OnSceneGUI()
        {
            if (DrawableAnimation != null && DrawableAnimation.Type == Animation.AnimationType.Position)
            {
                DrawHandles();
            }
        }

        private void DrawHandles()
        {
            EditorGUI.BeginChangeCheck();

            DrawLabel(DrawableAnimation.StartPosition + _labelOffset, _startPositionHandleName, _startPositionHandleColor);
            DrawLabel(DrawableAnimation.EndPosition + _labelOffset, _endPositionHandleName, _endPositionHandleColor);

            Vector3 endPosition = Handles.PositionHandle(DrawableAnimation.EndPosition, Quaternion.identity);
            Vector3 newPosition = Handles.PositionHandle(DrawableAnimation.StartPosition, Quaternion.identity);
            Handles.DrawLine(newPosition, endPosition);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(serializedObject.targetObject, _animationRecordName);
                DrawableAnimation.SetStartPosition(newPosition);
                DrawableAnimation.SetEndPosition(endPosition);
            }
        }

        private void DrawLabel(Vector3 position, string label, Color color)
        {
            GUIStyle labelStyle = new();
            labelStyle.fontSize = _labelFontSize;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = color;

            Handles.Label(position, label, labelStyle);
        }
    }
}