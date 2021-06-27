using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerRaceBegin : MonoBehaviour
{
    [SerializeField] UnityEvent onButtonPlayClicked;

    private void OnEnable()
    {
        MainMenuController.GetInstance().OnRaceReady += ()=> onButtonPlayClicked?.Invoke();
    }

    private void OnDisable()
    {
        MainMenuController.GetInstance().OnRaceReady -= () => onButtonPlayClicked?.Invoke();
    }
}