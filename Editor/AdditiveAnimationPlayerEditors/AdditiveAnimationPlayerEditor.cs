
namespace AnimationPlayers
{
    using UnityEditor;

    [CustomEditor(typeof(AdditiveAnimationPlayer))]
    public class AdditiveAnimationPlayerEditor : Editor
    {
        private readonly string _animationFieldName = "TargetAnimation";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(_animationFieldName));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
