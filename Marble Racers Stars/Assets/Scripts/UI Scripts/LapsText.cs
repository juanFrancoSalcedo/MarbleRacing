using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapsText : MonoBehaviour
{
    TextMeshProUGUI textLaps;

    void Start()
    {
        textLaps = GetComponent<TextMeshProUGUI>();
        RaceController.Instance.marblePlayerInScene.onLapWasSum += ShowLaps;
    }

    private void ShowLaps()
    {
        if (RaceController.Instance.marblePlayerInScene.currentMarbleLap <= RaceController.Instance.lapsLimit)
            textLaps.text = "LAP "+ RaceController.Instance.marblePlayerInScene.currentMarbleLap + "/" + RaceController.Instance.lapsLimit;
        else
            textLaps.text = "END";
    }
}
