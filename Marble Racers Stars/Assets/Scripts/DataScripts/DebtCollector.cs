using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class DebtCollector : MonoBehaviour
{
    [SerializeField] protected DataController dataManager = null;
    //[SerializeField] private DataShowText showMoney = null;
    //[SerializeField] private DataShowText otheShowMoney = null;
    [SerializeField] private TextMeshProUGUI textCondition = null;
    [SerializeField] private TextMeshProUGUI textPreviousCup = null;
    [SerializeField] private TextMeshProUGUI textMoneyneeded = null;
    [SerializeField] private GameObject secondPilotObj = null;
    [SerializeField] private TextMeshProUGUI textTrophies = null;
    [SerializeField] private Button buttonCharge = null;
    [Header("--------  Events  --------")]
    [SerializeField] private UnityEvent OnShowDebtCalled = null;
    public int trophiesNecesity { get; set; } = 3;
    public int debt { get; set; } = 100;
    public string previousCupPasses { get; set; } = "";
    public string curretnCupName { get; set; } = "";
    public bool secondPilot { get; set; } = false;
    public System.Action OnTrhopyWasUnlocked;

    void Awake()
    {
        buttonCharge.onClick.AddListener(Charge);
    }

    public void ShowDebtRequeriments()
    {
        textCondition.text = textCondition.text.Replace("@", curretnCupName);
        DisplayDebt();
        DisplayNecesities();
        DisplaySecondPilot();
        DisplayPreviousCup();
        OnShowDebtCalled?.Invoke();
        CheckCanIPayDebt();
    }

    #region Methods Dsiplay
    private void DisplayPreviousCup()
    {
        if (previousCupPasses.Equals(""))
            textPreviousCup.transform.parent.gameObject.SetActive(false);
        else
        {
            textPreviousCup.transform.parent.gameObject.SetActive(true);
            textPreviousCup.text = "Win the " + previousCupPasses;
        }
    }

    private void DisplaySecondPilot()
    {
        if (secondPilot)
            secondPilotObj.transform.gameObject.SetActive(true);
        else
            secondPilotObj.transform.gameObject.SetActive(false);
    }

    private void DisplayNecesities()
    {
        if (trophiesNecesity == 0)
            textTrophies.transform.parent.gameObject.SetActive(false);
        else
        {
            textTrophies.transform.parent.gameObject.SetActive(true);
            textTrophies.text = "" + trophiesNecesity;
        }
    }

    private void DisplayDebt()
    {
        if (debt == 0)
            textMoneyneeded.transform.parent.gameObject.SetActive(false);
        else
        {
            textMoneyneeded.transform.parent.gameObject.SetActive(true);
            textMoneyneeded.text = "" + debt;
        }
    }
    #endregion

    private void CheckCanIPayDebt()
    {
        int money = dataManager.GetMoney();
        int trophies = dataManager.GetTrophys();
        bool cupPassed =  (dataManager.allCups.GetIndexLeagueByName(previousCupPasses)<=dataManager.GetCupsWon() )?true:false;
        bool driverSecondUnlocked = (dataManager.GetSpecificKeyInt(KeyStorage.SECOND_PILOT_UNLOCKED_I) == 1);
        bool secondDriver = false;
        if (!secondPilot)
            secondDriver = driverSecondUnlocked;
        else
            secondDriver = true;

        print((money>=debt).ToString().ToUpper()+" MONEY");
        print((trophies>=trophiesNecesity).ToString().ToUpper()+" Trophies");
        print(cupPassed.ToString().ToUpper()+" CUPS");
        print(secondDriver.ToString().ToUpper()+" Second Driver");
        buttonCharge.interactable = (money >= debt && trophies >= trophiesNecesity && cupPassed && secondDriver);
    }

    public bool CheckCanIPay(int simulatedDebt, int simulateTrophies, string simulateCupName, bool simulateSecondDriver)
    {
        int money = dataManager.GetMoney();
        int trophies = dataManager.GetTrophys();
        bool cupPassed = (dataManager.allCups.GetIndexLeagueByName(simulateCupName) <= dataManager.GetCupsWon()) ? true : false;
        
        bool secondDriver = dataManager.GetSpecificKeyInt(KeyStorage.SECOND_PILOT_UNLOCKED_I) == 1;
        return (money >= simulatedDebt && trophies >= simulateTrophies && cupPassed && simulateSecondDriver == secondDriver);
    }

    void Charge()
    {
        //HIERARCHY
        MoneyManager.Transact(-debt);
        MoneyManager.UpdateMoney();
        dataManager.UnlockCup();
        OnTrhopyWasUnlocked?.Invoke();
        buttonCharge.interactable = false;
    }
}
