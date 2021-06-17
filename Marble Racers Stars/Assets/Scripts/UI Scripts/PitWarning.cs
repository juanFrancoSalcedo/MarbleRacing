using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using TMPro;

public class PitWarning : Singleton<PitWarning>
{
    TextMeshProUGUI textPro => GetComponent<TextMeshProUGUI>();
    DoAnimationController animationController => GetComponent<DoAnimationController>();
    private void Awake()
    {
        DisableWarningPits();
    }
    public void ActiveWarningPits() 
    {
        textPro.enabled = true;
        animationController.ActiveAnimation();
    }

    public void DisableWarningPits() 
    {
        textPro.enabled = false;
        animationController.StopAnimations();
    }
}
