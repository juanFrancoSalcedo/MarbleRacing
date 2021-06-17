using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.UI;
public class WarningTrophyChange : Singleton<WarningTrophyChange>
{
    [SerializeField] Button buttonYes = null;
    private TrophyUnlocker currentTrophy = null;
    private void Start() 
    {
        buttonYes.onClick.AddListener(SelectCurrentCup);
    }
    public void ChooseCup(TrophyUnlocker unlocker) 
    {
        currentTrophy = unlocker;
        GetComponent<DoAnimationController>().ActiveAnimation();
    } 
    public void SelectCurrentCup() 
    {
        currentTrophy.ActiveCurrenCupIsThis();
    }
}