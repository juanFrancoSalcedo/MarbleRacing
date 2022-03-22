using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScaleAspectRatioFitter : MonoBehaviour
{
    [SerializeField] private CanvasScaler scaler;
    [SerializeField] private float lessScale;

    private bool effect = false;

    private void OnValidate()
    {
        if (effect)
        { 
            UpdateRectTransform();
            effect = false;
        }
    }

    void Update()
    {
        UpdateRectTransform();
    }

    private void UpdateRectTransform() 
    {
        if (Screen.width > Screen.height)
            transform.localScale = Vector3.one * ((Screen.width / scaler.referenceResolution.x) - lessScale);
        else
            transform.localScale = Vector3.one;
    }
    
}
