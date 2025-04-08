namespace AnimationPlayers
{
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;

    [CustomEditor(typeof(AnimationPlayer))]
    public class AnimationPlayerEditor : BasePlayerEditor
    {
        protected string AnimationsFieldName = "Animations";
        protected List<Animation> Animations = new();
        protected SerializedProperty ListProperty;

        private string _typeFieldName = "_type";
        private ReorderableList _list;


        private void OnEnable()
        {
            Initialize();

            _list = new ReorderableList(serializedObject, ListProperty);
            _list.drawHeaderCallback += (rect) => EditorGUI.LabelField(rect, AnimationsFieldName);
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
            _list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void Initialize()
        {
            ListProperty = serializedObject.FindProperty(AnimationsFieldName);

            AnimationPlayer player = target as AnimationPlayer;
            FieldInfo animationsField = player.GetType().GetField(AnimationsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            Animations = animationsField.GetValue(player) as List<Animation>;
        }

        private void AddElement(ReorderableList list)
        {
            Animations.Add(new());
            SceneView.RepaintAll();
        }

        private void RemoveElement(ReorderableList list)
        {
            if (DrawableAnimation == Animations[list.index])
            {
                DrawableAnimation = null;
                SceneView.RepaintAll();
            }

            Animations.RemoveAt(list.index);
        }

        private void ReorderList(ReorderableList list, int oldIndex, int newIndex)
        {
            Animation movedItem = Animations[oldIndex];
            Animations.RemoveAt(oldIndex);

            if (newIndex > oldIndex)
                newIndex--;

            Animations.Insert(newIndex, movedItem);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.indentLevel++;

            SerializedProperty element = ListProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element);

            if (element.isExpanded && (Animation.AnimationType)element.FindPropertyRelative(_typeFieldName).enumValueIndex == Animation.AnimationType.Position)
                DrawEditButton(new Rect(rect.x, rect.y + EditorGUI.GetPropertyHeight(element), rect.width, EditorGUIUtility.singleLineHeight), index);

            EditorGUI.indentLevel--;
        }

        private float GetElementHeight(int index)
        {
            SerializedProperty element = ListProperty.GetArrayElementAtIndex(index);

            if (element.isExpanded == false)
                return EditorGUIUtility.singleLineHeight;

            if ((Animation.AnimationType)element.FindPropertyRelative(_typeFieldName).enumValueIndex == Animation.AnimationType.Position)
                return EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            else
                return EditorGUI.GetPropertyHeight(element);
        }

        private void DrawEditButton(Rect position, int elementIndex)
        {
            bool isSelected = DrawableAnimation == Animations[elementIndex];
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
            DrawableAnimation = Animations[index];
        }
    }
}