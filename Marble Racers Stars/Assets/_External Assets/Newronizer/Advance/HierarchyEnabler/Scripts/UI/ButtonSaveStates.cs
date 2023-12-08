using Newronizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CallerNavigationRecordScan))]
public class ButtonSaveStates : BaseButtonAttendant
{    
    void Start()
    {
        var eventOnClick = buttonComponent.onClick;
        buttonComponent.onClick = new Button.ButtonClickedEvent();
        buttonComponent.onClick.AddListener(GetComponent<CallerNavigationRecordScan>().CallRecord);
        buttonComponent.onClick.AddListener(()=> eventOnClick?.Invoke());
    }
}
