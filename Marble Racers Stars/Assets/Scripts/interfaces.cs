using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMainExpected
{
    void SubscribeToMainMenu();
    void ReadyToPlay();
    //public void SubscribeToTheMainMenu()
    //{
    //    MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;
    //}
    //public void ReadyToPlay()
    //{

    //}
}

public interface IRacerSettingsRegistrable
{
    void SubscribeRacerSettings();
    void FillMyMarbles(List<Marble> marblesObteined);
}
