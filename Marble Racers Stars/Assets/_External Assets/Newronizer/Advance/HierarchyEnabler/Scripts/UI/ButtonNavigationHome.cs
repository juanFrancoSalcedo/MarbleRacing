using Newronizer.HierarchyStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigationHome : MonoBehaviour
{
    Button buttonComponent => GetComponent<Button>();

    private void Start()
    {
        buttonComponent.onClick.AddListener(Home);
    }

    private void Home() 
    {
        NavigationRecordController.Instance.ClearRecord();
        NavigationRecordController.Instance.Activator.RestoreObjects();
    }
}
