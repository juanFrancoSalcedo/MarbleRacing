using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DisplayLostQualy : MonoBehaviour
{
    [SerializeField] private string textDisplay;
    [SerializeField] private GameObject canvasInputs;
    void Start()
    {
        RaceController.Instance.onQualifiyingCompleted += PlayerHasLost;
        RaceController.Instance.OnPlayerArrived += (i)=>HideText();
    }

    void PlayerHasLost() 
    {
        if (!RaceController.Instance.marblePlayerInScene.isZombieQualy) return;
        canvasInputs.SetActive(false);
        ShowText();
    }
    void ShowText()
    {
        GetComponent<TextMeshProUGUI>().text = textDisplay;
    }

    void HideText() 
    {
        GetComponent<TextMeshProUGUI>().text = string.Empty;
    }

}
