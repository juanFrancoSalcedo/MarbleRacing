using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonUnlockSecondPilot : BaseButtonComponent
{
    [SerializeField] private Text textmesh = null;

    private void OnEnable()
    {
        textmesh.text = "600 \n Coins";
        MoneyManager.onMoneyUpdated += MoneyUpdated;
        MoneyUpdated();
        buttonComponent.onClick.AddListener(PaySecondPilot);
    }
    private void OnDisable()
    {
        MoneyManager.onMoneyUpdated -= MoneyUpdated;
    }

    private void MoneyUpdated() 
    {
        buttonComponent.interactable = Workshop.Instance.CheckCanPay(600);
    }
    void PaySecondPilot()
    {
        MoneyManager.Transact(-600);
        RaceController.Instance.dataManager.SetSpecificKeyInt(KeyStorage.SECOND_PILOT_UNLOCKED_I,1);
    }
}
