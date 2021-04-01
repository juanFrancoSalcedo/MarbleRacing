using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDeployMinimap : MonoBehaviour, IMainExpected
{
    [SerializeField] private GameObject map;
    private Button button => GetComponent<Button>();
    void Start()
    {
        SubscribeToTheMainMenu();
        button.onClick.AddListener(ShiftMap);
        button.image.enabled = false;
        button.enabled = false;
    }


    public void SubscribeToTheMainMenu() 
    {
        MainMenuController.GetInstance().OnRaceReady+= ReadyToPlay;
        RaceController.Instance.OnPlayerArrived += delegate { RaceEnded(); };
    }

    public void ReadyToPlay()
    {
        button.image.enabled = true;
        button.enabled = true;
    }

    public void RaceEnded() 
    {
        button.image.enabled = false;
        button.enabled = false;
        map.SetActive(false);
    }

    void ShiftMap() => map.SetActive(!map.activeInHierarchy);
}
