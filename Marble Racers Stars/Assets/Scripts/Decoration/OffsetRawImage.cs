using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffsetRawImage : MonoBehaviour
{
    [SerializeField] Vector2 speed;
    RawImage rawImage;
    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }
    float timeOffset = 0;
    void Update()
    {
        timeOffset += Time.deltaTime;
        rawImage.uvRect = new Rect (timeOffset*speed, rawImage.uvRect.size);
    }
}
