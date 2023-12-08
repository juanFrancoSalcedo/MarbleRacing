using Newronizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Newronizer.SceneLoader
{
    public class CallerSceneLoader : MonoBehaviour
    {
        public static List<string> listScenes = new List<string>();
        [ListToEnumEditor(typeof(CallerSceneLoader), nameof(listScenes))]
        public string sceneName;//= System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0));

        public void LoadScene()
        {
            SceneLoader.Instance.LoadScene(sceneName);
        }

        #region EditorMethods
        private void OnValidate()
        {
            ReadScenes();
        }

        [ContextMenu("Read Scenes")]
        public void ReadScenes()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            listScenes.Clear();

            for (int i = 0; i < sceneCount; i++)
            {
                listScenes.Add(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
            }
        }
        #endregion
    }
}