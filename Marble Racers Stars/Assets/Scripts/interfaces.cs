using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMainExpected
{
    void SubscribeToMainMenu();
    void ReadyToPlay();
}

public interface IRacerSettingsRegistrable
{
    void SubscribeRacerSettings();
    void FillMyMarbles(List<Marble> marblesObteined);
}
