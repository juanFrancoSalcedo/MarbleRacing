using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class UpdaterMarbleImage : MonoBehaviour, IRacerSettingsRegistrable
{
    Image spriteImageComp;
    [SerializeField] private int indexTeam;
    private void Start()
    {
        spriteImageComp = GetComponent<Image>();
        SubscribeRacerSettings();
    }

    public void SubscribeRacerSettings() 
    {
        if(RacersSettings.GetInstance()!= null)
            RacersSettings.GetInstance().onListFilled += FillMyMarbles;
    }

    public void FillMyMarbles(List<Marble> myMarbles) 
    {
            spriteImageComp.sprite = myMarbles[indexTeam].marbleInfo.spriteMarbl;
    }
}
