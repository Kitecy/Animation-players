using UnityEditor;

namespace AnimationPlayers
{
    [CustomEditor(typeof(AnimationPlayersGroup))]
    public class AnimationPlayersGroupEditor : Editor
    {
        private string _playersObjectsListField = "_playersObjects";
        private string _playOnEnableField = "_playOnEnable";
        private string _intervalField = "_interval";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(_playersObjectsListField));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(_playOnEnableField));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(_intervalField));
            serializedObject.ApplyModifiedProperties();
        }
    }
}