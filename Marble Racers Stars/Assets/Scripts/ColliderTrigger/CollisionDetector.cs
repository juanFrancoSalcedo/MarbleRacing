using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public LayerMask detectionLayer;
    public event System.Action<Collision> OnCollisionEntered;
    public event System.Action<Collision> OnCollisionStayed;
    public event System.Action<Collision> OnCollisionExited;


    private void OnCollisionEnter(Collision other)
    {
        if (LayerDetection.DetectContainedLayers(detectionLayer, other.gameObject))
        {
            OnCollisionEntered?.Invoke(other);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (LayerDetection.DetectContainedLayers(detectionLayer, other.gameObject))
        {
            OnCollisionStayed?.Invoke(other);
        }

    }

    private void OnCollisionExit(Collision other)
    {
        if (LayerDetection.DetectContainedLayers(detectionLayer, other.gameObject))
        {
            OnCollisionExited?.Invoke(other);
        }
    }

}
