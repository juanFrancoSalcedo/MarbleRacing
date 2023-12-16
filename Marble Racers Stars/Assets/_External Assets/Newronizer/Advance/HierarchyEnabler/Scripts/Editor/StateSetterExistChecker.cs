using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

namespace Newronizer.HierarchyStates
{
    #region UNITY_EDITOR
    [InitializeOnLoad]
    class StateSetterExistChecker
    {
        static StateSetterExistChecker()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if(!Application.isPlaying)
                CheckExistDuplicated();
        }

        private static void CheckExistDuplicated() 
        {
            var array = CustomHierarchy.stateObjects;
            if (array == null)
                return;
            foreach (var item in array)
            {
                var count = array.Where(i => i.reference.UniqueId.Equals(item.reference.UniqueId));
                if (count.Count() > 1)
                {
                    item.reference.UpdateElementToken();
                    #region UNITY_EDITOR
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    #endregion
                }
            }
        }
    }
    #endregion
}