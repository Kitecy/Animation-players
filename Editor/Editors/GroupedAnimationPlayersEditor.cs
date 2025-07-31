using AnimationPlayers.Players;
using UnityEditor;
using UnityEngine;

namespace AnimationPlayers.Editor
{
    [CustomEditor(typeof(GroupedAnimationPlayers))]
    public class GroupedAnimationPlayersEditor : BaseEditor
    {
        private readonly string _playerSettingsLabel = "Player settings";
        private readonly string _playersListFieldName = "_players";
        private readonly string _playerFieldName = "_player";
        private readonly string _intervalFieldName = "_interval";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty playerField = serializedObject.FindProperty(_playerFieldName);
            SerializedProperty playersListField = serializedObject.FindProperty(_playersListFieldName);
            SerializedProperty intervalField = serializedObject.FindProperty(_intervalFieldName);

            EditorGUILayout.LabelField(_playerSettingsLabel);
            EditorGUILayout.PropertyField(playerField);
            EditorGUILayout.PropertyField(intervalField);
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(playersListField);

            GameObject go = ((MonoBehaviour)target).gameObject;

            if (go.scene.IsValid() && go.scene.name != null)
            {
                EditorGUILayout.Space();

                if (GUILayout.Button("Find players from children"))
                    OnFindButtonClicked();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnFindButtonClicked()
        {
            (target as GroupedAnimationPlayers).SetPlayersFromChildren();
        }
    }
}
