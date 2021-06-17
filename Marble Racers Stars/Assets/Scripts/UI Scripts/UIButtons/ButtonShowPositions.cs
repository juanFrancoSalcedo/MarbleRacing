using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShowPositions : BaseButtonComponent
{
    [SerializeField] CanvasGroup group;

    void Start()
    {
        buttonComponent.onClick.AddListener(()=>group.alpha = (group.alpha==1)?0:1);
    }
}
