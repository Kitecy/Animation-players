using AnimationPlayers.Players;
using UnityEditor;

namespace AnimationPlayers.Editor
{
    [CustomEditor(typeof(SimpleAnimationPlayer))]
    [CanEditMultipleObjects]
    public class SimpleAnimationPlayerEditor : BaseEditor
    {
        private readonly string _animationFieldName = "_playableAnimation";

        private SerializedProperty _playableAnimationProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            _playableAnimationProp = serializedObject.FindProperty(_animationFieldName);
            DrawableAnimation = (target as SimpleAnimationPlayer).Animation;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(_playableAnimationProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
