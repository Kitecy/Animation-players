using AnimationPlayers.Players;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Animation = AnimationPlayers.Players.Animation;

namespace AnimationPlayers.Editor
{
    [CustomEditor(typeof(AnimationsPlayer))]
    [CanEditMultipleObjects]
    public class AnimationsPlayerEditor : BaseEditor
    {
        private readonly string _playerSettingsLabel = "Player settings";

        private string _animationsFieldName = "_animations";
        private string _animationsFieldLabel = "Animations";
        private SerializedProperty _listProperty;

        private ReorderableList _list;

        private AnimationsPlayer _player;

        protected override void OnEnable()
        {
            base.OnEnable();

            _player = (target as AnimationsPlayer);

            _listProperty = serializedObject.FindProperty(_animationsFieldName);

            _list = new ReorderableList(serializedObject, _listProperty);
            _list.drawHeaderCallback += (rect) => EditorGUI.LabelField(rect, _animationsFieldLabel);
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

            EditorGUILayout.LabelField(_playerSettingsLabel);
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            if (targets.Length <= 1)
                _list.DoLayoutList();
            else
                EditorGUILayout.LabelField(MultipleEditingError);

            serializedObject.ApplyModifiedProperties();
        }

        private void AddElement(ReorderableList list)
        {
            foreach (var t in targets)
            {
                SerializedObject player = new SerializedObject(t);

                SerializedProperty listProperty = player.FindProperty(_animationsFieldName);

                listProperty.arraySize++;

                SerializedProperty newElement = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);

                newElement.managedReferenceValue = new Animation();

                player.ApplyModifiedProperties();
                SceneView.RepaintAll();
            }
        }

        private void RemoveElement(ReorderableList list)
        {
            foreach (var t in targets)
            {
                SerializedObject player = new SerializedObject(t);
                SerializedProperty listProperty = player.FindProperty(_animationsFieldName);

                IReadOnlyAnimation animation = (t as AnimationsPlayer).Animations[list.index];

                if (DrawableAnimation == animation)
                {
                    DrawableAnimation = null;
                    SceneView.RepaintAll();
                }

                listProperty.DeleteArrayElementAtIndex(list.index);

                player.ApplyModifiedProperties();
            }
        }

        private void ReorderList(ReorderableList list, int oldIndex, int newIndex)
        {
            foreach (var t in targets)
            {
                SerializedObject player = new SerializedObject(t);
                SerializedProperty listProperty = player.FindProperty(_animationsFieldName);

                listProperty.MoveArrayElement(oldIndex, newIndex);

                player.ApplyModifiedProperties();
            }
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.indentLevel++;

            SerializedProperty element = _listProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element);

            Rect position = new Rect(rect.x, rect.y + EditorGUI.GetPropertyHeight(element), rect.width, EditorGUIUtility.singleLineHeight);

            if (element.isExpanded)
            {
                if (targets.Length <= 1)
                {
                    Animation animation = element.managedReferenceValue as Animation;

                    if (animation != null && animation.Sort == Animation.Type.Position)
                        DrawEditButton(position, index);
                }
                else
                {
                    EditorGUI.LabelField(position, MultipleEditingError);
                }
            }

            EditorGUI.indentLevel--;
        }

        private float GetElementHeight(int index)
        {
            SerializedProperty element = _listProperty.GetArrayElementAtIndex(index);

            if (element.isExpanded == false)
                return EditorGUIUtility.singleLineHeight;

            Animation animation = element.managedReferenceValue as Animation;

            if (animation != null && animation.Sort == Animation.Type.Position)
                return EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            else
                return EditorGUI.GetPropertyHeight(element);
        }

        private void DrawEditButton(Rect position, int elementIndex)
        {
            bool isSelected;

            if (DrawableAnimation == null)
                isSelected = false;
            else
                isSelected = DrawableAnimation == _player.Animations[elementIndex];

            string text = isSelected ? "Stop edit" : "Edit";

            if (GUI.Button(position, text))
            {
                if (isSelected == false)
                    SelectElement(elementIndex);
                else
                    DrawableAnimation = null;

                SceneView.RepaintAll();
            }
        }

        private void SelectElement(int index)
        {
            DrawableAnimation = _player.Animations[index];
        }
    }
}
