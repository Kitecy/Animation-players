using AnimationPlayers.Players;
using UnityEditor;

namespace AnimationPlayers.Editor
{
    [CustomEditor(typeof(AnimationsPlayersQueue))]
    [CanEditMultipleObjects]
    public class AnimationsPlayersQueueEditor : BaseEditor
    {
        private readonly string _playersListFieldName = "_players";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty playersListField = null;

            playersListField = serializedObject.FindProperty(_playersListFieldName);

            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(playersListField);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
