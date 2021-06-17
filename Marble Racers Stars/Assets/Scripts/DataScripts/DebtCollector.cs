using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebtCollector : MonoBehaviour
{
    [SerializeField] protected DataManager dataManager = null;
    //[SerializeField] private DataShowText showMoney = null;
    //[SerializeField] private DataShowText otheShowMoney = null;
    [SerializeField] private TextMeshProUGUI textCondition = null;
    [SerializeField] private TextMeshProUGUI textPreviousCup = null;
    [SerializeField] private TextMeshProUGUI textMoneyneeded = null;
    [SerializeField] private GameObject secondPilotObj= null;
    [SerializeField] private TextMeshProUGUI textTrophies = null;
    [SerializeField] private Button buttonCharge = null;
    public int trophiesNecesity { get; set; } = 3;
    public int debt { get; set; } = 100;
    public string previousCupPasses{ get; set; } = "";
    public string curretnCupName{ get; set; } = "";
    public bool secondPilot{ get; set; } = false;
    public System.Action OnTrhopyWasUnlocked;

    void Awake()
    {
        buttonCharge.onClick.AddListener(Charge);
    }

    private void OnEnable()
    {
        ShowDebtRequeriments();   
    }

    void ShowDebtRequeriments()
    {
        textCondition.text = textCondition.text.Replace("@",curretnCupName);

        if (debt == 0)
        {
            textMoneyneeded.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            textMoneyneeded.transform.parent.gameObject.SetActive(true);
            textMoneyneeded.text = "" + debt;
        }

        if (trophiesNecesity == 0)
        {
            textTrophies.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            textTrophies.transform.parent.gameObject.SetActive(true);
            textTrophies.text = "" + trophiesNecesity;
        }

        if (secondPilot)
            secondPilotObj.transform.parent.gameObject.SetActive(true);
        else
            secondPilotObj.transform.parent.gameObject.SetActive(false);

        if (previousCupPasses.Equals(""))
        {
            textPreviousCup.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            textPreviousCup.transform.parent.gameObject.SetActive(true);
            textPreviousCup.text = "Win the " + previousCupPasses;
        }

        CheckCanIPayDebt();
    }

    private void CheckCanIPayDebt()
    {
        int money = dataManager.GetMoney();
        int trophies = dataManager.GetTrophys();
        bool cupPassed =  (dataManager.allCups.GetIndexLeagueByName(previousCupPasses)<=dataManager.GetCupsWon() )?true:false;
        bool secondDriver = dataManager.GetSpecificKeyInt(KeyStorage.SECOND_PILOT_UNLOCKED_I) == 1 && secondPilot || 
            dataManager.GetSpecificKeyInt(KeyStorage.SECOND_PILOT_UNLOCKED_I) == 0 && !secondPilot;
        if (money >= debt && trophies >= trophiesNecesity && cupPassed && secondDriver)
        {
            buttonCharge.interactable = true;
        }
        else
        {
            buttonCharge.interactable = false;
        }
    }

    public bool CheckCanIPay(int simulatedDebt, int simulateTrophies, string simulateCupName, bool simulateSecondDriver)
    {
        int money = dataManager.GetMoney();
        int trophies = dataManager.GetTrophys();
        bool cupPassed = (dataManager.allCups.GetIndexLeagueByName(simulateCupName) <= dataManager.GetCupsWon()) ? true : false;
        bool secondDriver = dataManager.GetSpecificKeyInt(KeyStorage.SECOND_PILOT_UNLOCKED_I) == 1;
        if (money >= simulatedDebt && trophies >= simulateTrophies && cupPassed && simulateSecondDriver == secondDriver)
            return true;
        else
            return false;
    }

    void Charge()
    {
        //HIERARCHY
        MoneyManager.Transact(-debt);

        MoneyManager.UpdateMoney();
        //if (showMoney != null)
        //    showMoney.CalculateAndShowData();
        //if (otheShowMoney != null)
        //    otheShowMoney.CalculateAndShowData();
        
        dataManager.UnlockCup();
        OnTrhopyWasUnlocked?.Invoke();
    }
}
