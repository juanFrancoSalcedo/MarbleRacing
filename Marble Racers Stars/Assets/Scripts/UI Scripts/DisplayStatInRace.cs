using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LeagueSYS;

public class DisplayStatInRace : MonoBehaviour
{
    [SerializeField] protected Image fillImageStat = null;
    [SerializeField] protected string nameOfStat = "forceTurbo";
    [SerializeField] protected string nameOfConstant = "forceBaseForward";
    [SerializeField] protected int idPlayer = 71;
    protected Pilot pilot = new Pilot();

    public void UpdateStats(int idPilot)
    {
        idPlayer = idPilot;
        pilot = PilotsDataManager.Instance.GetListPilots().listPilots.Find(x => x.ID == idPlayer);
        //int _money = Workshop.Instance.CallDisplayDebt(pilotPlayer.stats, nameOfStat, nameOfConstant);
        int part = PilotsStatsSetter.GetFractionalOfStat(pilot.stats, nameOfStat, nameOfConstant);
        fillImageStat.DOFillAmount((float)part / Constants.fractionStats, 0.9f);
    }
}
