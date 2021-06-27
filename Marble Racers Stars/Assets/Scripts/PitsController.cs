using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class PitsController : Singleton<PitsController>, IMainExpected
{
    public TypeCovering coveringType = TypeCovering.Medium;
    [SerializeField] GameObject mainMenuGroup;
    [SerializeField] GameObject[] quadsSelection;

    private void Start()
    {
        if (RacersSettings.GetInstance().leagueManager.Liga.GetUsingWear())
        { 
            mainMenuGroup.SetActive(true);
            SubscribeToMainMenu();
            SetPlayerCoveringOnPits("Medium");
        }
    }

    #region IMainSpected Methods
    public void SubscribeToMainMenu()=>MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;
    public void ReadyToPlay()=> ActivePitsController();

    #endregion
    public void SetPlayerCoveringOnPits(string nameCovering)
    {
        coveringType =(TypeCovering)System.Enum.Parse(typeof(TypeCovering), nameCovering);
        System.Array.ForEach(quadsSelection, x=>x.gameObject.SetActive(false));
        int coveringIndex = (int)coveringType;
        quadsSelection[coveringIndex].gameObject.SetActive(true);
    }
    public void ActivePitsController() 
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        group.alpha = 1;
        group.interactable=true;
        group.blocksRaycasts=true;
    }
}
