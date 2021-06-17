using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class PitsController : Singleton<PitsController>, IMainExpected
{
    public TypeCovering coveringType = TypeCovering.Medium;
    [SerializeField] GameObject mainMenuGroup;

    private void Start()
    {
        if (RacersSettings.GetInstance().leagueManager.Liga.GetUsingWear())
        { 
            mainMenuGroup.SetActive(true);
            SubscribeToMainMenu();
        }
    }

    #region IMainSpected Methods
    public void SubscribeToMainMenu()=>MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;
    public void ReadyToPlay()=> ActivePitsController();

    #endregion
    public void SetPlayerCoveringOnPits(string nameCovering)
    {
        coveringType =(TypeCovering)System.Enum.Parse(typeof(TypeCovering), nameCovering);
    }
    public void ActivePitsController() 
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        group.alpha = 1;
        group.interactable=true;
    }
}
