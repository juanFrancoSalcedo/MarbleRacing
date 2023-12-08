using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapsText : MonoBehaviour
{
    TextMeshProUGUI textLaps;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        textLaps = GetComponent<TextMeshProUGUI>();
        if(RaceController.Instance != null)
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
