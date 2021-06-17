using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalMotion : MonoBehaviour
{

    [SerializeField] bool xAxis = true;
    [SerializeField] bool yAxis = false;
    [SerializeField] bool zAxis = false;
    [SerializeField] float multiplier =0;
    private Vector3 originalPos;
    private Vector3 originalLocalPos;
    [SerializeField] bool useLocalPos;

    void Start()
    {
        originalPos = transform.position;
        originalLocalPos = transform.localPosition;
    }

    void Update()
    {
        if (!useLocalPos)
        {
            transform.position = new Vector3
            (originalPos.x +((xAxis) ? Mathf.Cos(Time.time) * multiplier : 0),
            originalPos.y + ((yAxis) ? Mathf.Cos(Time.time) * multiplier : 0),
            originalPos.z + ((zAxis) ? Mathf.Cos(Time.time) * multiplier : 0));
        }
        else
        {
            transform.localPosition = new Vector3
          (originalLocalPos.x + ((xAxis) ? Mathf.Cos(Time.time) * multiplier : 0),
          originalLocalPos.y + ((yAxis) ? Mathf.Cos(Time.time) * multiplier : 0),
          originalLocalPos.z + ((zAxis) ? Mathf.Cos(Time.time) * multiplier : 0));
        }
    }
}
