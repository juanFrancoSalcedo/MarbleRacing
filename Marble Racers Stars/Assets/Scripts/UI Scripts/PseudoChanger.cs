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
    [SerializeField] [ConditionalField(nameof(teamName),true)] private int idOfPlayer;
    [SerializeField] private bool isForSecondPlayer = false;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        SettingsPseudo();
        inputField.onEndEdit.AddListener(ChangeName);
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
    
    void  ChangeName(string textIntroduced)
    {
        if (teamName)
            PlayerPrefs.SetString(KeyStorage.NAME_PLAYER, textIntroduced);
        else
        {
            Pilot _pilot = PilotsDataManager.Instance.SelectPilot(idOfPlayer);
            _pilot.namePilot = textIntroduced;
            if(isForSecondPlayer)
                RaceController.Instance.SecondPlayerInScene.namePilot = textIntroduced;
            else
                RaceController.Instance.marblePlayerInScene.namePilot = textIntroduced;
            PilotsDataManager.Instance.UpdatePilot(_pilot);
        }
    }
}
