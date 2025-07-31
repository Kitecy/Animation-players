using System;
using System.Linq;
using UnityEditor;

namespace AnimationPlayers.Editor
{
    public static class PackageChecker
    {
        public static readonly string _saveKey = "AnimationPlayers_DependenciesChecked";

        static PackageChecker()
        {
            EditorApplication.update += CheckDependenciesOnce;
        }

        private static void CheckDependenciesOnce()
        {
            EditorApplication.update -= CheckDependenciesOnce;

            // ѕровер€ем, показывали ли уже предупреждение
            if (EditorPrefs.GetBool(_saveKey, false))
                return;

            bool hasDotween = TypeExists("DG.Tweening.DOTween");
            bool hasUniTask = TypeExists("Cysharp.Threading.Tasks.UniTask");

            if (!hasDotween || !hasUniTask)
            {
                DependencyWarningWindow.Open();
            }

            EditorPrefs.SetBool(_saveKey, true);
        }

        private static bool TypeExists(string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Any(assembly => assembly.GetType(typeName) != null);
        }
    }
}
