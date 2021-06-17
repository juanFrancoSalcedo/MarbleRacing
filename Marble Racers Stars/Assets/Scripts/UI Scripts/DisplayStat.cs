using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LeagueSYS;


public class DisplayStat : MonoBehaviour
{
    [SerializeField] protected Image fillImageStat = null;
    [SerializeField] private Text textAmount = null;
    [SerializeField] private Button btnPay = null;
    [SerializeField] protected string nameOfStat = "forceTurbo";
    [SerializeField] protected string nameOfConstant = "forceBaseForward";
    [SerializeField] protected int idPlayer = 71;
    protected Pilot pilotPlayer = new Pilot();

    private void Start()
    {
        btnPay.onClick.AddListener(PayTech);
        UpdateStats();
        //StartCoroutine(Actualizar());
    }

    private void OnEnable()=>MoneyManager.onMoneyUpdated += UpdateStats;

    private void OnDisable()=>MoneyManager.onMoneyUpdated -= UpdateStats;

  

    private void PayTech() 
    {
        int _money = Workshop.Instance.CallDisplayDebt(pilotPlayer.stats, nameOfStat, nameOfConstant);
        var item = pilotPlayer.stats.GetType().GetField(nameOfStat)?.GetValue(pilotPlayer.stats);            
        var item2 = System.Type.GetType("Constants")?.GetField(nameOfConstant)?.GetValue(null);
        if (RaceController.Instance.dataManager.GetMoney() < _money)
        {
            return;
        }
        float result = float.Parse(item.ToString()) + float.Parse(item2.ToString());
        int? intResult = null;
        if (int.TryParse(result.ToString(), out int parsed))
            intResult = parsed;
        if (intResult != null)
        {
            pilotPlayer.stats.GetType().GetField(nameOfStat)?.SetValue(pilotPlayer.stats, (intResult));
        }
        else 
        {
            pilotPlayer.stats.GetType().GetField(nameOfStat)?.SetValue(pilotPlayer.stats, (result));
        }
        Workshop.Instance.Charge(pilotPlayer, -_money);
        Invoke("UpdateStats", 0.2f);
    }
    private void UpdateStats()
    {
        pilotPlayer = PilotsDataManager.Instance.GetListPilots().listPilots.Find(x => x.ID == idPlayer);
        int _money = Workshop.Instance.CallDisplayDebt(pilotPlayer.stats, nameOfStat, nameOfConstant);
        int part = PilotsStatsSetter.GetFractionalOfStat(pilotPlayer.stats, nameOfStat, nameOfConstant);
        
        fillImageStat.DOFillAmount((float)part / Constants.fractionStats, 0.9f);
        if (!PilotsStatsSetter.CheckCanUpdate(part))
        {
            textAmount.text = "Completed";
            btnPay.interactable = false;
            return;
        }
        textAmount.text = "" + _money;
        btnPay.interactable = Workshop.Instance.CheckCanPay(_money);
    }
}
