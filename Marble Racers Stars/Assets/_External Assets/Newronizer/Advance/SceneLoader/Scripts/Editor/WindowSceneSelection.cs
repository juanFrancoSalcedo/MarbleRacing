using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;


namespace Newronizer.SceneLoader
{
    public class WindowSceneSelection : EditorWindow
    {
#if UNITY_EDITOR
        [MenuItem("Newronizer/SceneLoader/Window Scenes %F1")]
        public static void DisplayOne() => WindowSceneSelection.OpenWindow();
        public static string previousScenePath;
        private int tab = 0;
        private void OnGUI()
        {
            tab = GUILayout.Toolbar(tab, new string[] { "Build", "All" });
            Dictionary<string, string> pathsScenes = new Dictionary<string, string>();
            if (tab == 0)
                DisplayTabBuild(pathsScenes);
            else
                DisplayTabAll(pathsScenes);
        }

        private void DisplayTabAll(Dictionary<string, string> pathsScenes)
        {
            var scenes = AssetDatabase.FindAssets("t:Scene");
            foreach (var item in scenes)
            {
                string pathScene = AssetDatabase.GUIDToAssetPath(item);
                pathsScenes.Add(pathScene, GetNameSceneByPath(pathScene));
            }

            foreach (var item in pathsScenes)
            {
                if (GUILayout.Button(item.Value))
                    OpenScene(item.Key);
            }
        }

        private void DisplayTabBuild(Dictionary<string, string> pathsScenes)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string pathScene = SceneUtility.GetScenePathByBuildIndex(i);
                pathsScenes.Add(pathScene, GetNameSceneByPath(pathScene));
            }

            foreach (var item in pathsScenes)
            {
                if (GUILayout.Button(item.Value))
                    OpenScene(item.Key);
            }
        }

        private static void OpenScene(string path)
        {
            previousScenePath = EditorSceneManager.GetActiveScene().path;
            EditorSceneManager.OpenScene(path);
            CloseWindow();
        }

        public static void OpenWindow()
        {
            EditorWindow window = GetWindow(typeof(WindowSceneSelection));
            window.Show();
        }

        public static void CloseWindow()
        {
            EditorWindow window = GetWindow(typeof(WindowSceneSelection));
            window.Close();
        }

        private string GetNameSceneByPath(string path)
        {
            string nameScene = "";
            string[] values = path.Split(new char[] { '/' });
            foreach (var item in values)
            {
                if (item.Contains(".unity"))
                    nameScene = item;
            }
            return nameScene.Replace(".unity", "");
        }
#endif
    }

}
