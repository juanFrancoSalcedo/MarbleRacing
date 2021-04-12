using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using MyBox;

public class DataShowText : MonoBehaviour
{
    [SerializeField] private DataManager dtaManager;
    [SerializeField] TextMeshProUGUI textOfData;
    [SerializeField] TextMeshPro textInWorld;
    public TypeDataShow dataShow;
    public bool showInAwake = true;

    private void Start()
    {
        if (showInAwake)
        {
            CalculateAndShowData();
        }
    }

    public void CalculateAndShowData()
    {
        switch (dataShow)
        {
            case TypeDataShow.Money:

                if (dtaManager.GetTransactionMoney() != 0)
                    StartCoroutine(Transaction());
                else
                    textOfData.text = "" + dtaManager.GetMoney();
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
        int startAmount= dtaManager.GetMoney();
        int _moneyPlus = dtaManager.GetTransactionMoney();
        int endAmount =startAmount + _moneyPlus;
        int amountPerIteration =(int) (_moneyPlus / 19);
        dtaManager.DeleteTransaction();

        while (count <11)
        {
            startAmount += amountPerIteration;
            textOfData.text = "" + startAmount;
           yield return new WaitForSeconds(0.03f);
            count++;
        }
        dtaManager.DepositMoney(_moneyPlus);
        textOfData.text = "" + endAmount;
    }

    public enum TypeDataShow
    {
        Money,
        Trophys,
        Date,
        NameCup
    }
}

