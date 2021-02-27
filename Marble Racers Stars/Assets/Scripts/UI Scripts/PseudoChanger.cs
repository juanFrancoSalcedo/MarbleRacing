using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class PseudoChanger : MonoBehaviour
{
    TMP_InputField inputField;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        if (PlayerPrefs.GetString(KeyStorage.NAME_PLAYER).Equals(""))
        {
            inputField.text = (Constants.NORMI);
        }
        else
        {
            inputField.text = PlayerPrefs.GetString(KeyStorage.NAME_PLAYER);
        }

        inputField.onEndEdit.AddListener(ChangeName);
    }
    
    void  ChangeName(string textIntroduced)
    {
        PlayerPrefs.SetString(KeyStorage.NAME_PLAYER,textIntroduced);
    }
}
