﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MiniMapDisplay : MonoBehaviour
{
    [SerializeField] Vector2 sizeLandscape = new Vector2(500, 500);
    [SerializeField] Vector2 sizePortrait = new Vector2(300, 700);

    void Start()
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
        { 
            GetComponent<RectTransform>().sizeDelta = sizePortrait;
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        { 
            GetComponent<RectTransform>().sizeDelta = sizeLandscape;
        }
    }

    private void OnEnable()
    {
        if(Screen.width > Screen.height)
            GetComponent<RectTransform>().sizeDelta = sizeLandscape;
        else if (Screen.height > Screen.width)
            GetComponent<RectTransform>().sizeDelta = sizePortrait;

    }
    public void ShiftOrientation()
    {
        if (GetComponent<RectTransform>().sizeDelta == sizePortrait)
        {
            GetComponent<RectTransform>().sizeDelta = sizeLandscape;
        }
        else if (GetComponent<RectTransform>().sizeDelta == sizeLandscape)
        {
            GetComponent<RectTransform>().sizeDelta = sizePortrait;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Mini Map/ Shift Orientation")]
    static void SetOrientations() 
    {
        MiniMapDisplay[] array = GameObject.FindObjectsOfType<MiniMapDisplay>();
        foreach (var item in array)
            item.ShiftOrientation();
        
    }
#endif
}
