namespace AnimationPlayers
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public class BasePlayerEditor : Editor
    {
        [NonSerialized] protected Animation DrawableAnimation = null;

        protected string AnimationRecordName = "Animation Edit";

        protected Vector3 LabelOffset = new(0, 0.4f, 0);
        protected int LabelFontSize = 30;

        protected string StartPositionHandleName = "Start";
        protected Color StartPositionHandleColor = Color.green;

        protected string EndPositionHandleName = "End";
        protected Color EndPositionHandleColor = Color.red;

        protected void OnSceneGUI()
        {
            if (DrawableAnimation != null && DrawableAnimation.Type == Animation.AnimationType.Position)
            {
                DrawHandles();
            }
        }

        protected virtual void DrawHandles()
        {
            EditorGUI.BeginChangeCheck();

            DrawLabel(DrawableAnimation.StartPosition + LabelOffset, StartPositionHandleName, StartPositionHandleColor);
            DrawLabel(DrawableAnimation.EndPosition + LabelOffset, EndPositionHandleName, EndPositionHandleColor);

            Vector3 endPosition = Handles.PositionHandle(DrawableAnimation.EndPosition, Quaternion.identity);
            Vector3 startPosition = Handles.PositionHandle(DrawableAnimation.StartPosition, Quaternion.identity);
            Handles.DrawLine(startPosition, endPosition);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(serializedObject.targetObject, AnimationRecordName);
                DrawableAnimation.SetStartPosition(startPosition);
                DrawableAnimation.SetEndPosition(endPosition);
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