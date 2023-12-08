using System;
using UnityEngine;

namespace Newronizer.HierarchyStates
{
    //is monobehaviour in case a button wants use it
    public class HierarchyActivator : MonoBehaviour
    {
        public void RestoreObjectsByParent(Transform parent, bool justChildrens)
        {
            var compon = parent.GetComponentsInChildren<StateSetter>();
            Array.ForEach(compon, c => c.EnableOnHierarchy());
            if (parent.GetComponent<StateSetter>() && !justChildrens)
                parent.GetComponent<StateSetter>().EnableOnHierarchy();
        }

        public void RestoreObjects()
        {
            StateSetter[] compon = null;
#if UNITY_2019
            compon = Resources.FindObjectsOfTypeAll<StateSetter>();
#else
        compon = FindObjectsOfType<StateSetter>(true);
#endif
            Array.ForEach(compon, c => c.EnableOnHierarchy());
        }
    }
}