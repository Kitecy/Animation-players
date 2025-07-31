using UnityEditor;
using UnityEngine;

namespace AnimationPlayers.Editor
{
    public class DependencyWarningWindow : EditorWindow
    {
        private static string s_title = "Dependency warning window";

        private readonly string _errorMessage = "One of the required packages was not installed. Please make sure that you have installed all the necessary packages! Using the buttons below, you can copy the link to github of the latest versions of packages or to the package installation site.";
        private readonly int _fontSize = 15;

        private readonly string _dotweenButtonText = "DOTween installation site";
        private readonly string _uniTaskButtonText = "UniTask GitHub link";

        private readonly string _dotweenInstallLink = "https://assetstore.unity.com/packages/tools/visual-scripting/dotween-pro-32416";
        private readonly string _uniTaskInstallLink = "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask";

        public static void Open()
        {
            DependencyWarningWindow window = GetWindow<DependencyWarningWindow>();
            window.titleContent = new GUIContent(s_title);
        }

        private void OnGUI()
        {
            GUIStyle centeredLabelStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                fontSize = _fontSize
            };

            EditorGUILayout.LabelField(_errorMessage, centeredLabelStyle);

            EditorGUILayout.Space();

            if (GUILayout.Button(_dotweenButtonText))
                OpenLink();

            if (GUILayout.Button(_uniTaskButtonText))
                CopyToClipboard(_uniTaskInstallLink);
        }

        private void CopyToClipboard(string message)
        {
            EditorGUIUtility.systemCopyBuffer = message;
            Debug.Log("Link copied!");
        }

        private void OpenLink()
        {
            Application.OpenURL(_dotweenInstallLink);
        }
    }
}
