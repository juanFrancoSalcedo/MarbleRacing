using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMainExpected
{
    void SubscribeToTheMainMenu();
    void ReadyToPlay();
}

public interface IRacerSettingsRagistrable
{
    void SubscribeRacerSettings();
    void FillMyMarbles(List<Marble> marblesObteined);
}
