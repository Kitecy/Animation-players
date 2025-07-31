using UnityEditor;

namespace AnimationPlayers.Editor
{
    public class AnimationsPlayersQueueEditor : UnityEditor.Editor
    {
        private readonly string _playersListFieldName = "_players";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty playersListField = serializedObject.FindProperty(_playersListFieldName);

            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(playersListField);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
