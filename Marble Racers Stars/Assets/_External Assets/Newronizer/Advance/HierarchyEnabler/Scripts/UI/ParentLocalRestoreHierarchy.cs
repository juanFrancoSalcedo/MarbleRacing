using UnityEngine;

namespace Newronizer.HierarchyStates
{
    public class ParentLocalRestoreHierarchy: MonoBehaviour
    {
        [SerializeField] private Transform transformToRestore;
        [SerializeField] private bool justDisableChildren = true;
        public void LocalRestore() => NavigationRecordController.Instance.Activator.RestoreObjectsByParent(transformToRestore, justDisableChildren);
    }
}