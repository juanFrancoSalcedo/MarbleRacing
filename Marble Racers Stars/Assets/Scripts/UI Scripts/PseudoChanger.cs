using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyBox;
using LeagueSYS;

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
        inputField.onEndEdit.AddListener((string s)=>ChangeName());
    }

    private void SettingsPseudo() 
    {
        if (teamName)
        {
            if (abbreviation)
            {
                if (PlayerPrefs.GetString(KeyStorage.ABBREVIATION_PLAYER).Equals(""))
                    inputField.text = "NOR";
                else
                { 
                    //print("GET abb = " + PlayerPrefs.GetString(KeyStorage.ABBREVIATION_PLAYER, inputField.text));
                    inputField.text = PlayerPrefs.GetString(KeyStorage.ABBREVIATION_PLAYER);
                }
            }
            else
            {
                if (PlayerPrefs.GetString(KeyStorage.NAME_PLAYER).Equals(""))
                    inputField.text = (Constants.NORMI);
                else
                { 
                    inputField.text = PlayerPrefs.GetString(KeyStorage.NAME_PLAYER);
                    //print("GET TEAM = " + PlayerPrefs.GetString(KeyStorage.NAME_PLAYER, inputField.text));
                }
            }
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
        //ChangeName();
    }

    private void ChangeName()
    {
        if (teamName)
        {
            if (abbreviation)
            {
                PlayerPrefs.SetString(KeyStorage.ABBREVIATION_PLAYER, inputField.text);
                inputField.text = PlayerPrefs.GetString(KeyStorage.ABBREVIATION_PLAYER);
                //print("AB= "+PlayerPrefs.GetString(KeyStorage.ABBREVIATION_PLAYER, inputField.text));
            }
            else
            { 
                PlayerPrefs.SetString(KeyStorage.NAME_PLAYER, inputField.text);
                inputField.text = PlayerPrefs.GetString(KeyStorage.NAME_PLAYER);
                //print("TEAM = " + PlayerPrefs.GetString(KeyStorage.NAME_PLAYER, inputField.text));
            }
        }
        else
        {
            Pilot _pilot = PilotsDataManager.Instance.SelectPilot(idOfPlayer);
            _pilot.namePilot = inputField.text;
            if(isForSecondPlayer)
                RaceController.Instance.SecondPlayerInScene.namePilot = inputField.text;
            else
                RaceController.Instance.marblePlayerInScene.namePilot = inputField.text;
            PilotsDataManager.Instance.UpdatePilot(_pilot);
        }
    }
}
