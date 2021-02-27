using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerDetection
{
    public static bool DetectContainedLayers (LayerMask mask, GameObject objectDetected)
    {
        if (mask == (mask | 1 << objectDetected.layer))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
