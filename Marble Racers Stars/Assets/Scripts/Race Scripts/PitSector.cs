using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitSector : MonoBehaviour
{
    TriggerDetector detector;
    void Awake()
    {
        detector = GetComponent < TriggerDetector >();
        detector.OnTriggerEntered += MarbleEnter;
        detector.OnTriggerExited += MarbleExit;
        if(!RacersSettings.GetInstance().Broadcasting())
            ButtonPits.Instance.SetPitSector(this);
    }
    void Start()
    {
       gameObject.SetActive(LeagueManager.LeagueRunning.GetUsingWear());
    }

    private void MarbleEnter(Transform other) 
    {
        if (other.GetComponent<Marble>()) 
        {
            Marble marbInside = other.GetComponent<Marble>();
            marbInside.InPitStop = true;
            if (marbInside.isPlayer && marbInside.CheckUsedAllItPitStops() && !RacersSettings.GetInstance().Broadcasting())
                ButtonPits.Instance.EnableButton(true);
        }
    }

    private void MarbleExit(Transform other)
    {
        if (other.GetComponent<Marble>())
        {
            Marble marbInside = other.GetComponent<Marble>();
            marbInside.InPitStop = false;
            if (marbInside.isPlayer && !RacersSettings.GetInstance().Broadcasting())
                ButtonPits.Instance.EnableButton(false);
        }
    }
}
