using UnityEditor;
using UnityEngine;

namespace Newronizer.HierarchyStates
{
    public class HierarchyEditor : EditorWindow
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/Change Color Herarchy", false, 12)]
        public static void ShowWindow()
        {
            GetWindow<HierarchyEditor>("HierarchyEditor");
        }

        [MenuItem("GameObject/State/Add %e", false, 11)]
        public static void AddState()
        {
            Selection.activeGameObject.AddComponent(typeof(StateSetter));
        }

        [MenuItem("GameObject/State/Remove", false, 11)]
        public static void RemoveState()
        {
            DestroyImmediate(Selection.activeGameObject.GetComponent<StateSetter>());
        }

        private Color bufferColor = new Color(0.6f, 0.2f, 0.6f, 0.4f);
        private void OnGUI()
        {
            bufferColor = EditorGUILayout.ColorField("Color to " + Selection.activeGameObject.name, bufferColor);

            if (GUILayout.Button("Apply Color"))
            {
                if (!Selection.activeGameObject.GetComponent<ColorHierarchySetter>())
                    Selection.activeGameObject.AddComponent(typeof(ColorHierarchySetter));
                Selection.activeGameObject.GetComponent<ColorHierarchySetter>().colorInHierarchy = bufferColor;
                this.Close();
            }

            if (Selection.activeGameObject.GetComponent<ColorHierarchySetter>())
            {
                if (GUILayout.Button("Remove Color"))
                {
                    DestroyImmediate(Selection.activeGameObject.GetComponent<ColorHierarchySetter>());
                    this.Close();
                }
            }
        }
#endif
    }
}