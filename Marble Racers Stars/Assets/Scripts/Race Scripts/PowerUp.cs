﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class PowerUp : MonoBehaviour
{
    [SerializeField] TriggerDetector triggerDetect = null;
    [SerializeField] TextMeshPro textPowInside = null;
    [SerializeField] bool randomized = false;
    public PowerUpType typePower;
    [SerializeField] private GameObject onTriggerParticles = null;
    [SerializeField] private GameObject normalParticles = null;
    Quaternion rotationInit;
    

    void Start()
    {
        triggerDetect.OnTriggerEntered += GivePower;
        rotationInit = textPowInside.transform.rotation;
        Invoke("CheckCanActivePowerUps",0.1f);
    }

    private void LateUpdate()
    {
        textPowInside.transform.rotation  = rotationInit;
        normalParticles.transform.rotation = rotationInit;
    }

    private void CheckCanActivePowerUps()
    {
        gameObject.SetActive(RaceController.Instance.usePowerUps);
        randomized = !RaceController.Instance.useSinglePower;
        if (!randomized)
            typePower = RaceController.Instance.typeSingle;
        RandomPow();
    }

    void GivePower(Transform other)
    {
        if (!other.GetComponent<Marble>()) return;
        other.GetComponent<Marble>().SetPowerUp(typePower);
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        textPowInside.enabled = false;
        Invoke("RestorePowerUp",8f);
        onTriggerParticles.SetActive(true);
        normalParticles.SetActive(false);
    }

    void RestorePowerUp()
    {
        RandomPow();
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        textPowInside.enabled = true;
        onTriggerParticles.SetActive(false);
        normalParticles.SetActive(true);
    }

    private void RandomPow()
    {
        int rando = 0;

        if(randomized)
            rando = UnityEngine.Random.Range(1, Enum.GetNames(typeof(PowerUpType)).Length);
        else
            rando = (int)typePower;

        switch (rando)
        {
            case 1:
                typePower = PowerUpType.Freeze;
                textPowInside.text = Constants.freezeWord;
                textPowInside.color = Constants.freezeColor;
                break;
            //SMALL BEHAVIOUR
            case 2:
                typePower = PowerUpType.Shrink;
                textPowInside.text = Constants.shrinkWord;
                textPowInside.color = Constants.shrinkColor;
                break;

            case 3:
                typePower = PowerUpType.Enlarge;
                textPowInside.text = Constants.enlargeWord;
                textPowInside.color = Constants.enlargeColor;
                break;

            case 4:
                typePower = PowerUpType.Explo;
                textPowInside.text = Constants.exploUpWord;
                textPowInside.color = Constants.exploUpColor;
                break;

            case 5:
                typePower = PowerUpType.Wall;
                textPowInside.text = Constants.wallWord;
                textPowInside.color = Constants.wallColor;
                break;

            case 6:
                typePower = PowerUpType.Bump;
                textPowInside.text = Constants.bumpWord;
                textPowInside.color = Constants.bumpColor;
                break;
        }   
    }
}
