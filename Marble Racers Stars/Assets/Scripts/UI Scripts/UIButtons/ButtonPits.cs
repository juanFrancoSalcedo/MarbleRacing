using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class ButtonPits : Singleton<ButtonPits>,IMainExpected
{
    Button buttonComponent;
    public PitSector pitStop { get; private set; }
    
    void Awake() 
    {
        buttonComponent = GetComponent<Button>();
        buttonComponent.onClick.AddListener(SendPlayerRestoreMarble);
        SubscribeToMainMenu();
    }

    void Start() 
    {
        gameObject.SetActive(RacersSettings.GetInstance().leagueManager.Liga.GetUsingWear());
    }

    public void SubscribeToMainMenu()
    {
        MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;
    }
    public void ReadyToPlay()
    {
        buttonComponent.image.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void EnableButton(bool isEnable)=> buttonComponent.interactable = isEnable;
    public void SetPitSector(PitSector sector) => pitStop = sector;

    private void SendPlayerRestoreMarble() => RaceController.Instance.marblePlayerInScene.PitStop(PitsController.Instance.coveringType);

}
