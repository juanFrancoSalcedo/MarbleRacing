using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;
using MyBox;
using System;

public class Workshop :Singleton<Workshop>
{
    private void Start()
    {

    }
    public bool CheckCanPay(int price) 
    {
        int money = RaceController.Instance.dataManager.GetMoney();
        return money >= price;
    }

    public void Charge(Pilot pilotNew, int money) 
    {
        MoneyManager.Transact(money);
        PilotsDataManager.Instance.UpdatePilot(pilotNew);
        MoneyManager.UpdateMoney();
        RaceController.Instance.marblePlayerInScene.SetStats(pilotNew.stats);
        PilotsStatsSetter.SetARandomPilotStats();
    }

    public int CallDisplayDebt(MarbleStats _stats, string _nameStat, string _nameConstant) 
    {
        return GetDebtForImprove(PilotsStatsSetter.GetFractionalOfStat(_stats, _nameStat, _nameConstant));
    }

    private int GetDebtForImprove(int countFractions)
    {
        int x = Constants.baseMoney;
        while (countFractions > 0)
        {
            int sum = (int)(Mathf.Log10(x * 11) *x - (x * 1.5f));
            x = sum;
            countFractions--;
        }

        int figure = x.ToString().Length/2;
        while (figure >= 1)
        {
            string result = x.ToString();
            result = result.Replace(result[result.Length - figure], '0');
            x = int.Parse(result);
            figure--;
        }

        return x;
    }
}
