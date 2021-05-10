using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebtCollector : MonoBehaviour
{
    [SerializeField] protected DataManager dataManager;
    [SerializeField] private DataShowText showMoney;
    [SerializeField] private DataShowText otheShowMoney;
    [SerializeField] private TextMeshProUGUI textCondition;
    [SerializeField] private TextMeshProUGUI textPreviousCup;
    [SerializeField] private TextMeshProUGUI textMoneyneeded;
    [SerializeField] private TextMeshProUGUI textTrophies;
    [SerializeField] private Button buttonCharge;
    public int trophiesNecesity { get; set; } = 3;
    public int debt { get; set; } = 100;
    public string previousCupPasses{ get; set; } = "";
    public string curretnCupName{ get; set; } = "";
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
        int money = (dataManager.GetMoney()-dataManager.GetTransactionMoney());
        int trophies = dataManager.GetTrophys();
        bool cupPassed =  (dataManager.allCups.GetIndexLeagueByName(previousCupPasses)<=dataManager.GetCupsWon() )?true:false;
        if (money >= debt && trophies >= trophiesNecesity && cupPassed)
        {
            buttonCharge.interactable = true;
        }
        else
        {
            buttonCharge.interactable = false;
        }
    }

    public bool CheckCanIPay(int simulatedDebt, int simulateTrophies, string simulateCupName)
    {
        int money = (dataManager.GetMoney() - dataManager.GetTransactionMoney());
        int trophies = dataManager.GetTrophys();
        bool cupPassed = (dataManager.allCups.GetIndexLeagueByName(simulateCupName) <= dataManager.GetCupsWon()) ? true : false;

        if (money >= simulatedDebt && trophies >= simulateTrophies && cupPassed)
            return true;
        else
            return false;
    }

    void Charge()
    {
        //HIERARCHY
        dataManager.SetTransactionMoney(-debt);

        if (showMoney != null)
            showMoney.CalculateAndShowData();
        if (otheShowMoney != null)
            otheShowMoney.CalculateAndShowData();
        
        dataManager.UnlockCup();
        OnTrhopyWasUnlocked?.Invoke();
    }
}
