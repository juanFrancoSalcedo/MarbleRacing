using UnityEngine;

public static class MoneyManager
{
    public static event System.Action onMoneyUpdated;
    public static void UpdateMoney() 
    {
        onMoneyUpdated?.Invoke();
    }

    public static void Transact(int depositMoney) 
    {
        GameObject.FindObjectOfType<DataController>().DepositMoney(depositMoney);
        UpdateMoney();
    }
}

