using AnimationPlayers.Players;
using UnityEditor;
using UnityEngine;

namespace AnimationPlayers.Editor
{
    [CustomEditor(typeof(SimpleAnimationPlayer))]
    public class SimpleAnimationPlayerEditor : BaseEditor
    {
        private readonly string _animationFieldName = "_playableAnimation";

        private void OnEnable()
        {
            DrawableAnimation = serializedObject.FindProperty(_animationFieldName);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            SerializedProperty playableAnimationField = serializedObject.FindProperty(_animationFieldName);

            EditorGUILayout.PropertyField(playableAnimationField);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
