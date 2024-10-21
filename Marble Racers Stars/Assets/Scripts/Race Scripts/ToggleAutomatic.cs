using UnityEngine;
using Newronizer;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Toggle))]
public class ToggleAutomatic:Singleton<ToggleAutomatic>
{
    Toggle toggle => GetComponent<Toggle>();
    public static bool IsAutomatic = false;
    private void Start()
    {
        toggle.onValueChanged.AddListener(SetAutomatic);
        IsAutomatic = toggle.isOn;
    }

    private void SetAutomatic(bool arg0)
    {
        IsAutomatic = arg0;
    }
}