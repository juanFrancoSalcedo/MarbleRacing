using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private static MainMenuController Instance = null;
    public event System.Action OnRaceReady;
    [SerializeField] private Animator trafficAnimator = null;

    public static MainMenuController GetInstance()
    {
        if (Instance == null)
        {
            Instance = GameObject.FindObjectOfType<MainMenuController>();
        }
        return Instance;
    }

    public void PrepareRace()
    {
        Invoke("RaceBegin", 0.3f);
        trafficAnimator.SetBool("RaceBegin",true);
    }
    
    public void RaceBegin()
    {
        OnRaceReady?.Invoke();
    }
}
