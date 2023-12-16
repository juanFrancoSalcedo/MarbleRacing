using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIncreaseRace : BaseButtonComponent
{
    void Start()=>buttonComponent.onClick.AddListener(TryPlayVideo);

    private void TryPlayVideo() 
    {
        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //    RaceController.Instance.IncreaseLapLimit(); 
        Time.timeScale = 1.0f;
    }
}
