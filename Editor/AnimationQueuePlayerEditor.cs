using UnityEditor;

namespace AnimationPlayers
{
    [CustomEditor(typeof(AnimationQueuePlayer))]
    public class AnimationQueuePlayerEditor : Editor
    {
        private readonly string _playersListFieldName = "_animationPlayers";
        private readonly string _playOnEnableFieldName = "_playOnEnable";

        private SerializedProperty _playersListProperty;
        private SerializedProperty _playOnEnableProperty;

        private void OnEnable()
        {
            _playersListProperty = serializedObject.FindProperty(_playersListFieldName);
            _playOnEnableProperty = serializedObject.FindProperty(_playOnEnableFieldName);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_playersListProperty);
            EditorGUILayout.PropertyField(_playOnEnableProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}