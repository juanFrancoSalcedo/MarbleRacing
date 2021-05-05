using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

namespace Newronizer.CustomHierarchy
{

    [InitializeOnLoad]
    class CustomHierarchy
    {
#if UNITY_EDITOR
        static Texture2D textureDisable;
        static Texture2D textureEnable;
        static List<int> markedObjects = new List<int>();
        static StateSetter[] stateObjects;
        static ColorHierarchySetter[] colorObjects;
        static CustomHierarchy()
        {
            textureDisable = AssetDatabase.LoadAssetAtPath("Assets/Scripts/HierarchyEditor/Editor/Disabled.png", typeof(Texture2D)) as Texture2D;
            textureEnable = AssetDatabase.LoadAssetAtPath("Assets/Scripts/HierarchyEditor/Editor/Enabled.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.update += UpdateCB;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        }

        static void UpdateCB()
        {
            stateObjects = Resources.FindObjectsOfTypeAll<StateSetter>();
            colorObjects = Resources.FindObjectsOfTypeAll<ColorHierarchySetter>();

            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                //if (!EditorApplication.isPlaying)
                //SearchEnablers(stateObjects);
            }
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
                Rect rectRight = _selectionRect;
                rectLeft.x = 0;
                rectLeft.width = _selectionRect.x;
                rectRight.x = 100;

                EditorGUI.DrawRect(rectLeft, colorItem.colorInHierarchy);
                EditorGUI.DrawRect(rectRight, colorItem.colorInHierarchy);

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
                r.height = 12;
                r.x = r.width-10;
                r.width = 17;
                GUIContent _content;
                _content = new GUIContent("", (stateItem.stateEnable) ? textureEnable : textureDisable, "Check mark = Activado , \n Empty = Desactivado");
                if (GUI.Button(r, _content))
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