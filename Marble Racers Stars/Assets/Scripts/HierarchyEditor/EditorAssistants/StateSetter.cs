using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Newronizer.CustomHierarchy
{
    [DisallowMultipleComponent]
    public class StateSetter : MonoBehaviour
    {
        public bool stateEnable = false;
        private void OnValidate()
        {
            hideFlags = HideFlags.HideInInspector;
        }
        public void EnableOnHierarchy()
        {
            gameObject.SetActive(stateEnable);
        }
    }
}
