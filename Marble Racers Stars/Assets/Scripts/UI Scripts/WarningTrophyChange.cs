using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.UI;
public class WarningTrophyChange : MonoBehaviour
{
    [SerializeField] Button buttonYes;
    private TrophyUnlocker currentTrophy;
    public static WarningTrophyChange Instance;
    public WarningTrophyChange() 
    {
        Instance = this;
    }

    private void Start() 
    {
        buttonYes.onClick.AddListener(SelectCurrentCup);
    } 
    public void ChooseCup(TrophyUnlocker unlocker) => currentTrophy = unlocker;
    public void SelectCurrentCup() 
    {
        currentTrophy.ActiveCurrenCupIsThis();
    }
}