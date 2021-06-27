using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenObjectResize : MonoBehaviour
{
    [SerializeField] Vector2 sizePortrait;
    [SerializeField] Vector2 positionPortrait;
    [SerializeField] Vector2 sizeLandcape;
    [SerializeField] Vector2 positionLandscape;

    private ScreenOrientation lastOrientation;
    private float lastWidth;
    void Start()
    {
        SetOrientation();
    }
    void Update()
    {
        if (lastOrientation != Screen.orientation || Screen.width != lastWidth)
            SetOrientation();

        //print(UnityEngine.EventSystems.EventSystem.current.name);
    }

    private void SetOrientation() 
    {
        if (Screen.orientation == ScreenOrientation.Portrait && Screen.width < Screen.height)
        { 
            GetComponent<RectTransform>().anchoredPosition = positionPortrait;
            GetComponent<RectTransform>().sizeDelta = sizePortrait;
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = positionLandscape;
            GetComponent<RectTransform>().sizeDelta = sizeLandcape;
        }
        lastOrientation = Screen.orientation;
        lastWidth = Screen.width;
    }
}
