using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIncreaseRace : BaseButtonComponent
{
    void Start()=>buttonComponent.onClick.AddListener(AdsManager.Instance.PlayVideoReward);
}
