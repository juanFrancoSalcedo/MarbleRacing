﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PositionMarbleCanvas : MonoBehaviour, IMainExpected
{
    private Marble marbleTrans = null;
    private TextMeshProUGUI textPos = null;
    private Teleport teleport = null;
    private bool showing = false;
    [SerializeField] private TextMeshProUGUI powerText = null;
    private TextMeshProUGUI nameText = null;
    [SerializeField] private ImageMinimap miniMapIcon = null;
    Transform target;
    void Start()
    {
        target = Camera.main.transform;
        SubscribeToMainMenu();
        teleport = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Teleport>();
        teleport.OnExitPortal += ShowNumber;
        textPos = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        marbleTrans = GetComponentInParent<Marble>();
        marbleTrans.OnPowerUpDelivered += ShowMyPowerUp;
        marbleTrans.OnPowerUpObtained += ShowMyPowerUp;  
        transform.SetParent(null);
        if (marbleTrans.isZombieQualy)
            miniMapIcon.MarbleTrans = marbleTrans;
    }

    public void SubscribeToMainMenu()=>MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;

    public void ReadyToPlay() 
    {
        miniMapIcon.MarbleTrans = marbleTrans;
        showing = true;
    } 

    void Update()
    {
        if (marbleTrans.isZombieQualy) return;
        if (showing)
        {
            textPos.text = "" + (marbleTrans.boardController.transform.GetSiblingIndex() + 1);
            transform.position = marbleTrans.transform.position + new Vector3(0, 1, 0);
            nameText.text = marbleTrans.namePilot;//(marbleTrans.isPlayer) ? PlayerPrefs.GetString(KeyStorage.NAME_PLAYER) : marbleTrans.marbleInfo.nameMarble;
            transform.LookAt(target);
        }
        else
        {
            textPos.text = "";
            nameText.text = "";
        }
        if (!marbleTrans.gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }

    public void ShowNumber(Transform other)
    {
        if (ReferenceEquals(other, marbleTrans.transform))
        {
            showing = true;
        }
    }
    public void ShowNumber() => showing = true;


    private void ShowMyPowerUp(PowerUpType _powerUp)
    {
        switch (_powerUp)
        {
            case PowerUpType.Freeze:
                powerText.enabled = true;
                powerText.text = Constants.freezeWord;
                powerText.color = Constants.freezeColor;
                break;

            case PowerUpType.None:
                powerText.enabled = false;
                break;

            case PowerUpType.Enlarge:
                powerText.enabled = false;
                showing = false;
                Invoke("ShowNumber",Constants.timeBigSize);
                break;

            case PowerUpType.Explo:
                powerText.enabled = true;
                powerText.text = Constants.exploUpWord;
                powerText.color = Constants.exploUpColor;
                break;

            case PowerUpType.Bump:
                powerText.enabled = true;
                powerText.text = Constants.bumpWord;
                powerText.color = Constants.bumpColor;
                break;
        }
        Invoke("CheckCurrentPow",0.8f);
    }

    private void CheckCurrentPow() 
    {
        if (!marbleTrans.CheckHasPower()) 
        {
            powerText.enabled = false;  
        }
    }

}
