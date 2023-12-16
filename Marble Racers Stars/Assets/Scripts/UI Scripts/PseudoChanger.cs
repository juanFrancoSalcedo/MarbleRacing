using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyBox;
using LeagueSYS;
using System;

[RequireComponent(typeof(TMP_InputField))]
public class PseudoChanger : MonoBehaviour
{
    TMP_InputField inputField = null;
    [SerializeField] private bool teamName = true;
    [SerializeField] private bool abbreviation = false;
    [SerializeField] [ConditionalField(nameof(teamName),true)] private int idOfPlayer;
    [SerializeField] private bool isForSecondPlayer = false;
    void Start()
    {
        if(inputField == null)
        inputField = GetComponent<TMP_InputField>();
        SettingsPseudo();
        inputField.onValueChanged.AddListener(ChangeName);
    }

    private void ChangeName(string arg0)
    {
        if (teamName)
        {
            PlayerPrefs.SetString(KeyStorage.NAME_PLAYER, arg0);
        }
        else
        {
            Pilot _pilot = PilotsDataManager.Instance.SelectPilot(idOfPlayer);
            _pilot.namePilot = arg0;
            if (isForSecondPlayer)
                RaceController.Instance.SecondPlayerInScene.namePilot = arg0;
            else
                RaceController.Instance.marblePlayerInScene.namePilot = arg0;
            PilotsDataManager.Instance.UpdatePilot(_pilot);
        }
    }

    private void SettingsPseudo() 
    {
        if (teamName)
        {
            if (PlayerPrefs.GetString(KeyStorage.NAME_PLAYER).Equals(""))
                inputField.text = (Constants.NORMI);
            else
                inputField.text = PlayerPrefs.GetString(KeyStorage.NAME_PLAYER);
        }
        else
        {
            inputField.text = PilotsDataManager.Instance.SelectPilot(idOfPlayer).namePilot;
        }
    }
    /// <summary>
    /// pass th other input fiel to show the text
    /// </summary>
    /// <param name="textIntroduced"></param>
    public void ChangeNameFromOther(TMP_InputField other)
    {
        if(other.text.Length<=3)
            inputField.text = other.text;
    }
}
