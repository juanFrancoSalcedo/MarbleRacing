using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BSpecZoomCamera : MonoBehaviour
{
    public event System.Action<float> onScrollingZoom;

    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
            OnScroll(Input.mouseScrollDelta);
    }


    private void OnScroll(Vector2 scrollDir) 
    {
        onScrollingZoom?.Invoke(-scrollDir.y);
    }
}