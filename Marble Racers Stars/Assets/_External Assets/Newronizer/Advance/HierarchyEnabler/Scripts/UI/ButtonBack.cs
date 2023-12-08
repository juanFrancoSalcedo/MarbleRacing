using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newronizer.HierarchyStates
{
    public class ButtonBack : BaseButtonAttendant
    {
        void Start() => buttonComponent.onClick.AddListener(Back);
        private void Back() => NavigationRecordController.Instance.PopElement();
    }
}