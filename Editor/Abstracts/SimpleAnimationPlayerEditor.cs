using System.Reflection;
using UnityEditor;

namespace AnimationPlayers
{
    [CustomEditor(typeof(SimpleAnimationPlayer))]
    public class SimpleAnimationPlayerEditor : BasePlayerEditor
    {
        protected readonly string AnimationFieldName = "TargetAnimation";

        private void OnEnable()
        {
            InitializeAnimation();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(AnimationFieldName), true);
            serializedObject.ApplyModifiedProperties();
        }

        private void InitializeAnimation()
        {
            SimpleAnimationPlayer player = target as SimpleAnimationPlayer;
            FieldInfo animationFieldInfo = player.GetType().GetField(AnimationFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            DrawableAnimation = animationFieldInfo.GetValue(player) as Animation;
        }
    }
}