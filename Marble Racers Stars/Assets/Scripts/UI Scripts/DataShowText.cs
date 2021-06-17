using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using MyBox;

public class DataShowText : MonoBehaviour
{
    [SerializeField] private DataManager dtaManager = null;
    [SerializeField] TextMeshProUGUI textOfData = null;
    [SerializeField] TextMeshPro textInWorld = null;
    public TypeDataShow dataShow;
    public bool showInAwake = true;

    private void Start()
    {
        SubscribeUpdaters();
        if (showInAwake)
        {
            CalculateAndShowData();
        }
    }

    private void SubscribeUpdaters() 
    {
        switch (dataShow)
        {
            case TypeDataShow.Money:
                textOfData.text = dtaManager.GetMoney().ToString();
                MoneyManager.onMoneyUpdated +=CalculateAndShowData;
                break;
        }
    }

    private void OnDisable()
    {
        MoneyManager.onMoneyUpdated -= CalculateAndShowData;
    }

    private void OnDestroy()
    {
        MoneyManager.onMoneyUpdated -= CalculateAndShowData;
    }
    public void CalculateAndShowData()
    {
        switch (dataShow)
        {
            case TypeDataShow.Money:
                    StartCoroutine(Transaction());
                break;

            case TypeDataShow.Trophys:
                textOfData.text = "" + dtaManager.GetTrophys();
                break;

            case TypeDataShow.Date:
                StartCoroutine(WaitDateShow());
                break;

            case TypeDataShow.NameCup:
                if (textInWorld != null)
                    textInWorld.text = dtaManager.allCups.listCups[dtaManager.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I)].nameLeague;
                else
                    textOfData.text = dtaManager.allCups.listCups[dtaManager.GetSpecificKeyInt(KeyStorage.CURRENTCUP_I)].nameLeague;
                break;
        }
    }

    private IEnumerator WaitDateShow() 
    {
        yield return new WaitForSeconds(0.3f);
        textOfData.text = "" + SceneManager.GetActiveScene().name.Remove(0, 3) + " " +
            (CurrentLeague.LeagueRunning.date + 1) + "/" + CurrentLeague.LeagueRunning.listPrix.Count;
    }

    IEnumerator Transaction()
    {
        int count=0;
        int startAmount= int.Parse(textOfData.text);
        int _moneyPlus = dtaManager.GetMoney()-startAmount;
        int amountPerIteration =(int) (_moneyPlus / 18);

        while (count <18)
        {
            startAmount += amountPerIteration;
            textOfData.text = "" + startAmount;
            yield return new WaitForSeconds(0.04f);
            count++;
        }
        textOfData.text = "" + dtaManager.GetMoney();
    }

    public enum TypeDataShow
    {
        Money,
        Trophys,
        Date,
        NameCup
    }
}

