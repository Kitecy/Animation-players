using AnimationPlayers.Players;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Animation = AnimationPlayers.Players.Animation;

namespace AnimationPlayers.Editor
{
    [CustomEditor(typeof(AnimationsPlayer))]
    public class AnimationsPlayerEditor : BaseEditor
    {
        private readonly string _playerSettingsLabel = "Player settings";

        private string _animationsFieldName = "_animations";
        private string _animationsFieldLabel = "Animations";
        private List<Animation> _animations = new();
        private SerializedProperty _listProperty;

        private string _typeFieldName = "_type";
        private ReorderableList _list;

        private void Reset()
        {
            Initialize();
        }

        private void OnEnable()
        {
            Initialize();

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

            _list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void Initialize()
        {
            _listProperty = serializedObject.FindProperty(_animationsFieldName);

            AnimationsPlayer player = target as AnimationsPlayer;
            FieldInfo animationsField = player.GetType().GetField(_animationsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            _animations = animationsField.GetValue(player) as List<Animation>;
        }

        private void AddElement(ReorderableList list)
        {
            _animations.Add(new());
            SceneView.RepaintAll();
        }

        private void RemoveElement(ReorderableList list)
        {
            if (DrawableAnimation == _listProperty.GetArrayElementAtIndex(list.index))
            {
                DrawableAnimation = null;
                SceneView.RepaintAll();
            }

            _animations.RemoveAt(list.index);
        }

        private void ReorderList(ReorderableList list, int oldIndex, int newIndex)
        {
            Animation movedItem = _animations[oldIndex];
            _animations.RemoveAt(oldIndex);

            if (newIndex > oldIndex)
                newIndex--;

            _animations.Insert(newIndex, movedItem);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.indentLevel++;

            SerializedProperty element = _listProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element);

            if (element.isExpanded && (Animation.Type)element.FindPropertyRelative(_typeFieldName).enumValueIndex == Animation.Type.Position)
                DrawEditButton(new Rect(rect.x, rect.y + EditorGUI.GetPropertyHeight(element), rect.width, EditorGUIUtility.singleLineHeight), index);

            EditorGUI.indentLevel--;
        }

        private float GetElementHeight(int index)
        {
            SerializedProperty element = _listProperty.GetArrayElementAtIndex(index);

            if (element.isExpanded == false)
                return EditorGUIUtility.singleLineHeight;

            if ((Animation.Type)element.FindPropertyRelative(_typeFieldName).enumValueIndex == Animation.Type.Position)
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
                isSelected = DrawableAnimation.propertyPath == _listProperty.GetArrayElementAtIndex(elementIndex).propertyPath;

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
            DrawableAnimation = _listProperty.GetArrayElementAtIndex(index);
        }
    }
}
