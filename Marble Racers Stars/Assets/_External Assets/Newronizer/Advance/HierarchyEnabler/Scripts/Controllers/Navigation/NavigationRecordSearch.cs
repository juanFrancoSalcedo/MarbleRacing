using UnityEngine;
using System;
using System.Collections.Generic;

namespace Newronizer.HierarchyStates
{
    //is monobehaviour in case a button wants use it
    public class NavigationRecordSearch : MonoBehaviour
    {
        List<GameObject> bufferParents = new List<GameObject>();
        public void SearchElement(StateSetter element, bool disableParents)
        {
            Transform _parent = element.transform.parent;
            while (_parent != null)
            {
                _parent = WriteParent(_parent, disableParents);
            }
            element.SetActive(true);
        }

        private Transform WriteParent(Transform _parent, bool disableParents)
        {
            if (!_parent.gameObject.activeInHierarchy && disableParents)
                bufferParents.Add(_parent.gameObject);
            _parent.gameObject.SetActive(true);
            _parent = _parent.parent;
            return _parent;
        }
    }
}