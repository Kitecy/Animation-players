using AnimationPlayers.Players;
using UnityEditor;

namespace AnimationPlayers.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DynamicAnimationPlayer))]
    public class DynamicAnimationPlayerEditor : BaseEditor
    {
        private readonly string _isUIFieldName = "IsUI";

        private SerializedProperty _isUIProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _isUIProperty = serializedObject.FindProperty(_isUIFieldName);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_isUIProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
