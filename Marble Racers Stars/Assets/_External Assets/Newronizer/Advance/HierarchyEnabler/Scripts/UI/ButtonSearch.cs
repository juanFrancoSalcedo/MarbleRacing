using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Newronizer.HierarchyStates
{
    public class ButtonSearch : BaseButtonAttendant
    {
        [field: SerializeField] private StateSetter elementFound;
        [field: SerializeField] bool disableParents = false;

        private void Start()
        {
            buttonComponent.onClick.AddListener(Search);
        }

        private void Search()
        {
            NavigationRecordController.Instance.Searcher.SearchElement(elementFound, disableParents);
        }
    }
}