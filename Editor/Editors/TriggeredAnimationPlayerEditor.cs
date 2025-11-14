using AnimationPlayers.Players;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Animation = AnimationPlayers.Players.Animation;

namespace AnimationPlayers.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TriggeredAnimationPlayer))]
    public class TriggeredAnimationPlayerEditor : BaseEditor
    {
        private readonly string _isUIFieldName = "IsUI";
        private readonly string _triggersFieldLabel = "Triggers";
        private readonly string _triggersFieldName = "_triggers";

        private SerializedProperty _isUIProperty;

        private ReorderableList _list;

        private SerializedProperty _listProperty;

        protected override void OnEnable()
        {
            _isUIProperty = serializedObject.FindProperty(_isUIFieldName);

            _listProperty = serializedObject.FindProperty(_triggersFieldName);

            _list = new ReorderableList(serializedObject, _listProperty);
            _list.drawHeaderCallback += (rect) => EditorGUI.LabelField(rect, _triggersFieldLabel);
            _list.drawElementCallback += DrawElement;
            _list.onRemoveCallback += RemoveElement;
            _list.onAddCallback += AddElement;
            _list.onReorderCallbackWithDetails += ReorderList;
            _list.elementHeightCallback += GetElementHeight;
            _list.draggable = true;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_isUIProperty);

            if (targets.Length <= 1)
                _list.DoLayoutList();
            else
                EditorGUILayout.LabelField(MultipleEditingError);

            serializedObject.ApplyModifiedProperties();
        }

        private void AddElement(ReorderableList list)
        {
            foreach (Object t in targets)
            {
                SerializedObject player = new SerializedObject(t);
                SerializedProperty listProperty = player.FindProperty(_triggersFieldName);

                listProperty.arraySize++;

                SerializedProperty newElement = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);

                newElement.managedReferenceValue = new TriggerObject(null, new());

                player.ApplyModifiedProperties();
            }
        }

        private void RemoveElement(ReorderableList list)
        {
            foreach (Object t in targets)
            {
                SerializedObject player = new SerializedObject(t);
                SerializedProperty listProperty = player.FindProperty(_triggersFieldName);

                listProperty.DeleteArrayElementAtIndex(list.index);

                player.ApplyModifiedProperties();
            }
        }

        private void ReorderList(ReorderableList list, int oldIndex, int newIndex)
        {
            foreach (Object t in targets)
            {
                SerializedObject player = new SerializedObject(t);
                SerializedProperty listProperty = player.FindProperty(_triggersFieldName);

                listProperty.MoveArrayElement(oldIndex, newIndex);
                player.ApplyModifiedProperties();
            }
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.indentLevel++;

            SerializedProperty element = _listProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element, true);

            EditorGUI.indentLevel--;
        }

        private float GetElementHeight(int index)
        {
            SerializedProperty element = _listProperty.GetArrayElementAtIndex(index);

            if (element.isExpanded == false)
                return EditorGUIUtility.singleLineHeight;

            return EditorGUI.GetPropertyHeight(element);

            //Animation animation = element.managedReferenceValue as Animation;

            //if (animation != null && animation.Sort == Animation.Type.Position)
            //    return EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //else
            //    return EditorGUI.GetPropertyHeight(element);
        }
    }
}
