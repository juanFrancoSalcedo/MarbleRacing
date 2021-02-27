using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class TriggerDetector : MonoBehaviour
    {
        public LayerMask detectionLayer;
        public event System.Action<Transform> OnTriggerEntered;
        public event System.Action<Transform> OnTriggerExited;
        public event System.Action<Transform> OnTriggerStayed;

        private void OnTriggerEnter(Collider other)
        {
            if (LayerDetection.DetectContainedLayers(detectionLayer, other.gameObject))
            {
                OnTriggerEntered?.Invoke(other.transform);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (LayerDetection.DetectContainedLayers(detectionLayer, other.gameObject))
            {
                OnTriggerStayed?.Invoke(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (LayerDetection.DetectContainedLayers(detectionLayer, other.gameObject))
            {
                OnTriggerExited?.Invoke(other.transform);
            }
        }
    }


