using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class PitsController : Singleton<PitsController>, IMainExpected
{
    public TypeCovering bufferCovering = TypeCovering.Medium;
    private TypeCovering covering = TypeCovering.Medium;
    public TypeCovering CoveringType { get => covering; set { covering = value; OnCoveringUpdated?.Invoke(CoveringType); } }
    [SerializeField] GameObject mainMenuGroup = null;
    [SerializeField] GameObject[] quadsSelection = null;
    [SerializeField] private GameObject tutoCovering;
    public static event System.Action<TypeCovering> OnCoveringUpdated = null;

    private void Start()
    {
        if (LeagueManager.LeagueRunning.GetUsingWear() && !RacersSettings.GetInstance().Broadcasting())
        {
            if (PlayerPrefs.GetInt(KeyStorage.TUTO_COVERING_I, 0) == 0)
            { 
                tutoCovering.SetActive(true);
                PlayerPrefs.SetInt(KeyStorage.TUTO_COVERING_I, 1);
            }

            mainMenuGroup.SetActive(true);
            SubscribeToMainMenu();
            SetPlayerCoveringOnPits("Medium");
            OnCoveringUpdated?.Invoke(CoveringType);
        }
    }

    #region IMainSpected Methods
    public void SubscribeToMainMenu()=>MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;
    public void ReadyToPlay()=> ActivePitsController();

    #endregion
    public void SetPlayerCoveringOnPits(string nameCovering)
    {
        CheckIsRacing(nameCovering);
        System.Array.ForEach(quadsSelection, x=>x.gameObject.SetActive(false));
        int coveringIndex = (int)bufferCovering;
        quadsSelection[coveringIndex].gameObject.SetActive(true);
    }

    private void CheckIsRacing(string nameCovering)
    {
        if (RaceController.Instance.stateOfRace != RaceState.Racing)
            CoveringType = (TypeCovering)System.Enum.Parse(typeof(TypeCovering), nameCovering);
        else
            bufferCovering = (TypeCovering)System.Enum.Parse(typeof(TypeCovering), nameCovering);
    }

    public void ActivePitsController() 
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        group.alpha = 1;
        group.interactable=true;
        group.blocksRaycasts=true;
    }
}
