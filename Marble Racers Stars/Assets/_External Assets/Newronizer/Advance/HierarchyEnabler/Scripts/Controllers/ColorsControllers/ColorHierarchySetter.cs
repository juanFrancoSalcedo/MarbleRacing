using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newronizer.HierarchyStates
{
    [DisallowMultipleComponent]
    public class ColorHierarchySetter : MonoBehaviour
    {
        public Color colorInHierarchy = new Color(1, 1, 1, 1);

        private void OnValidate()
        {
            hideFlags = HideFlags.HideInInspector;
            if (colorInHierarchy.a > 0.5f)
                colorInHierarchy.a = 0.5f;
        }
        public void SetColor(Color newColor) => colorInHierarchy = newColor;

    }
}
