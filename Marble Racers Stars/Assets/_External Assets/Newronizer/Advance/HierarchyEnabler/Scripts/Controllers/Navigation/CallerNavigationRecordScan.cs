using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newronizer.HierarchyStates;

public class CallerNavigationRecordScan : MonoBehaviour
{
    public void CallRecord() => NavigationRecordController.Instance.ScanElements();
}
