using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;
using TMPro;

public class ButtonPits : Singleton<ButtonPits>,IMainExpected
{
    Button buttonComponent;
    public PitSector pitStop { get; private set; }
    [SerializeField] private Text textPits;
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
    }
    public void EnableButton(bool isEnable) 
    {
        textPits.gameObject.SetActive(isEnable);
        buttonComponent.interactable = isEnable;
    } 
    public void SetPitSector(PitSector sector) => pitStop = sector;

    private void SendPlayerRestoreMarble() => RaceController.Instance.marblePlayerInScene.PitStop(PitsController.Instance.coveringType);

}
