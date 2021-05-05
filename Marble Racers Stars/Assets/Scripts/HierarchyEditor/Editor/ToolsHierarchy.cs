using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Newronizer.CustomHierarchy 
{
    public class ToolsHierarchy
    {
        [MenuItem("Tools/Newronizer/Restore Object In Hierarchy")]
        private static void CallSearch()
        {
            CustomHierarchy.SearchEnablers();
        }
    }
}

