using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimatorParameter : BaseButtonComponent
{
    [SerializeField] private string parameterName = null;
    [SerializeField] private bool m_value = false;
    [SerializeField] private Animator animatorTarget = null;
    void Awake()
    {
        buttonComponent.onClick.AddListener(delegate { animatorTarget.SetBool(parameterName, m_value); });
    }
}
