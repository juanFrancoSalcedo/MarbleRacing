using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newronizer.HierarchyStates
{
    public class NavigationRecordInstaller : MonoBehaviour
    {
        private void OnValidate()
        {
            InstanciateActivator();
            InstanciateSearch();
        }

        private void InstanciateActivator()
        {
            if (transform.childCount == 0 || transform.GetChild(0) == null)
            {
                GameObject obj = new GameObject();
                obj.AddComponent(typeof(HierarchyActivator));
                obj.transform.SetParent(transform);
                obj.name = "HierarchyActivator";
            }
        }

        private void InstanciateSearch()
        {
            if (transform.childCount == 1 || transform.GetChild(1) == null)
            {
                GameObject obj = new GameObject();
                obj.AddComponent(typeof(NavigationRecordSearch));
                obj.transform.SetParent(transform);
                obj.name = "NavigationRecordSearch";
            }
        }
    }
}