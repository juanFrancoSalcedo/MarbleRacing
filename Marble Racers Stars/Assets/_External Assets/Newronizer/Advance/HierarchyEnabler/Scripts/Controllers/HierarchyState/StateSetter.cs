using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Newronizer.HierarchyStates
{
    [DisallowMultipleComponent]
    public class StateSetter : MonoBehaviour
    {
        public bool stateEnable = false;
        [SerializeField] private float timeDelay = 0f;
        [SerializeField] public StateReference reference = null;
        public static event System.Action<StateReference> OnEnabled;
        bool lastState = false;
        private void OnValidate()
        {
            #if UNITY_EDITOR
            if (reference == null)
                reference = new StateReference();
            if (string.IsNullOrEmpty(reference.UniqueId) ||
                string.IsNullOrEmpty(reference.sceneName) ||
                reference.handler == null)
            {
                reference.UpdateElementToken();
                reference.UpdateElementScene();
                reference.UpdateElementHandler(this);
                
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
            #endif
        }

        private void OnEnable()
        {
            if (!stateEnable)
                OnEnabled?.Invoke(reference);
        }

        public void EnableOnHierarchy() => gameObject.SetActive(stateEnable);

        public void SetActive(bool active)
        {
            if (active)
                gameObject.SetActive(active);
            else
                SetActiveFalse();
        }

        private void SetActiveFalse()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(WaitDisable());
        }

        private IEnumerator WaitDisable()
        {
            yield return new WaitForSeconds(timeDelay);
            gameObject.SetActive(false);
        }
    }
}
