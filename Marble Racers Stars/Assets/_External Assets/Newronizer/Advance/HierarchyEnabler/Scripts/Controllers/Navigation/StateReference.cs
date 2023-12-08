using System.Collections.Generic;
using System.Text;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

namespace Newronizer.HierarchyStates
{
    [System.Serializable]
    public class StateReference : IActivatable
    {
        public string UniqueId;
        public StateSetter handler;
        public string sceneName;

#if UNITY_EDITOR
        public void UpdateElementToken() => this.UniqueId = RandomToken.CreateRandomToken(12);
        public void UpdateElementScene() => this.sceneName = EditorSceneManager.GetActiveScene().name;
        public void UpdateElementHandler(StateSetter settes) => this.handler = settes;
#endif
        public bool SetActive(bool state)
        {
            if (!SceneLoader.SceneLoader.GetCurrentSceneName().Equals(sceneName))
            {
                SceneLoader.SceneLoader.Instance.LoadScene(sceneName);
                return false;
            }
            if (handler == null)
            {
                handler = RetrieveHandler();
            }
            handler.SetActive(state);
            return true;
        }

        public bool GetHierarchyState()
        {
            if (handler != null)
                return handler.gameObject.activeInHierarchy;
            else
            {
                Debug.Log("ELEMENTO NULO REVISAR");
                return false;
            }
        }

        private StateSetter RetrieveHandler()
        {
#if UNITY_2019
            var elementsFound = Resources.FindObjectsOfTypeAll<StateSetter>();
            return System.Array.Find(elementsFound, item => 
                item.gameObject.scene.isLoaded && item.reference.UniqueId.Equals(this.UniqueId)
            );

#else
            var elementsFound = GameObject.FindObjectsOfType<StateSetter>(true);
            return System.Array.Find(elementsFound, item => 
                item.reference.UniqueId.Equals(this.UniqueId)
            );
#endif
        }
    }
}
