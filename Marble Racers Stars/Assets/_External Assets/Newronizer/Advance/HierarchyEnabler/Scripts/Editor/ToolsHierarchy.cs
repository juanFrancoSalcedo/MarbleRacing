using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Newronizer.HierarchyStates 
{
    public class ToolsHierarchy
    {
        public static event System.Action OnRestored = null;

        [MenuItem("Newronizer/Navigation Hierarchy/Restore Object In Hierarchy %#e")]
        private static void CallSearch()
        {
            CustomHierarchy.SearchEnablers();
            OnRestored?.Invoke();
        }

    }
}

