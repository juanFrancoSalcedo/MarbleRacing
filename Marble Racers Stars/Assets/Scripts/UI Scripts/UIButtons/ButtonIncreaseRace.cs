using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIncreaseRace : MonoBehaviour
{
    Button buttonCompon;
    bool subscribed;

    void Start()
    {
        buttonCompon = GetComponent<Button>();
        buttonCompon.onClick.AddListener(RaceController.Instance.IncreaseLapLimit);
    }
}
