using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

namespace Newronizer.HierarchyStates
{

    [InitializeOnLoad]
    class CustomHierarchy
    {
#if UNITY_EDITOR

        public static StateSetter[] stateObjects;
        static ColorHierarchySetter[] colorObjects;
        static CustomHierarchy()
        {
            EditorApplication.update += UpdateCB;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        }

        static void UpdateCB()
        {
            stateObjects = Resources.FindObjectsOfTypeAll<StateSetter>();
            colorObjects = Resources.FindObjectsOfTypeAll<ColorHierarchySetter>();
        }

        public static void SearchEnablers()
        {
            foreach (var item in stateObjects)
            { 
                item.EnableOnHierarchy();
                EditorUtility.SetDirty(item);
                EditorSceneManager.MarkSceneDirty(item.gameObject.scene);
            }
        }

        static void HierarchyItemCB(int instanceID, Rect selectionRect)
        {
            DrawEnablerRemains(instanceID, selectionRect);
            DrawHierarchyColor(instanceID, selectionRect);
        }

        private static void DrawHierarchyColor(int _instanceID, Rect _selectionRect)
        {
            if (!TryGetColorSetter()) return;

            ColorHierarchySetter colorItem = null;

            if (TryGetStateSetter())
            {
                foreach (ColorHierarchySetter item in colorObjects)
                {
                    if (item != null && item.gameObject.GetInstanceID().Equals(_instanceID))
                    {
                        colorItem = item;
                        break;
                    }
                }
            }

            if (colorItem != null)
            {
                Rect rectLeft = _selectionRect;
                //Rect rectRight = _selectionRect;
                rectLeft.x = 20;
                rectLeft.width = _selectionRect.x/3;
                //rectRight.x = 100;

                EditorGUI.DrawRect(rectLeft, colorItem.colorInHierarchy);
                //EditorGUI.DrawRect(rectRight, colorItem.colorInHierarchy);

            }
        }

        private static void DrawEnablerRemains(int _instanceID, Rect _selectionRect)
        {
            if (!TryGetStateSetter()) return;

            StateSetter stateItem = null;

            if (TryGetStateSetter())
            {
                foreach (StateSetter item in stateObjects)
                {
                    if (item != null && item.gameObject.GetInstanceID().Equals(_instanceID))
                    {
                        stateItem = item;
                        break;
                    }
                }
            }

            if (stateItem != null)
            {
                Rect r = new Rect(_selectionRect);
                r.height = 16;
                r.x = r.width-10;
                r.width = 20;
                //GUIContent _content;
                //_content = new GUIContent("", (stateItem.stateEnable) ? textureEnable : textureDisable, "Check mark = Activado , \n Empty = Desactivado");
                if (GUI.Button(r, stateItem.stateEnable ?"O":""))
                    stateItem.stateEnable = !stateItem.stateEnable;
            }
        }

        static bool TryGetStateSetter()
        {
            if (stateObjects == null)
                return false;
            else
                return true;
        }

        static bool TryGetColorSetter()
        {
            if (colorObjects == null)
                return false;
            else
                return true;
        }
#endif
    }
}