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
        SubscribeToMainMenu();
        button.onClick.AddListener(ShiftMap);
        button.image.enabled = false;
        button.enabled = false;
    }
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.F1))
            ShiftMap();
        if (Input.GetKeyDown(KeyCode.F2))
            CameraMiniMap.Instance.ChangeMiniMap();
    }

    public void SubscribeToMainMenu() 
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

    void ShiftMap()
    {
        map.SetActive(!map.activeInHierarchy);
        DistanceDisplay.Instance.DisplayHideDistance();
    } 
}
