using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newronizer.HierarchyStates
{
    public class ButtonLocalRestore : BaseButtonAttendant
    {
        [SerializeField] private Transform transformToRestore;
        [SerializeField] private bool justDisableChildren = true;
        void Start() => buttonComponent.onClick.AddListener(LocalRestore);

        public void LocalRestore() => NavigationRecordController.Instance.Activator.RestoreObjectsByParent(transformToRestore, justDisableChildren);
    }
}