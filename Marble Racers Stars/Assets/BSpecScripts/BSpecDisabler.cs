using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpecDisabler : MonoBehaviour
{
    private void OnEnable()
    {
        if (RacersSettings.GetInstance().Broadcasting())
            gameObject.SetActive(false);
    }
}
