using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoomByAspectRatio : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam = null;
    [SerializeField] private float fieldViewPortrait = 0;
    [SerializeField] private float fieldViewLandscape =0;

    float widthBuffer;
    float heightBuffer;
    
    void Start()
    {
        widthBuffer = Screen.width;
        heightBuffer = Screen.height;
        CalculateScreenSize();
    }

    private void Update()
    {
        if (Screen.width != widthBuffer)
        {
            CalculateScreenSize();
            widthBuffer = Screen.width;
            heightBuffer = Screen.height;
        }
    }

    private void CalculateScreenSize() 
    {
        if (Screen.width > Screen.height)
            cam.m_Lens.FieldOfView = fieldViewLandscape;
        else
            cam.m_Lens.FieldOfView = fieldViewPortrait;
    }
}
