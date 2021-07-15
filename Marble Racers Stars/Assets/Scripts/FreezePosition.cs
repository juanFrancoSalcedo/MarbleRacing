using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePosition : MonoBehaviour
{
    [SerializeField] Vector3 bufferPosition;

    void Update()
    {
        if (bufferPosition != transform.localPosition)
            transform.localPosition = bufferPosition;
    }
}
