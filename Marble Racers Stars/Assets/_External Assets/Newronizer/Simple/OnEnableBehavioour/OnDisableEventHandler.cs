using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDisableEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent OnDisableCallback;
    [SerializeField] private float delay = 0;

    private void OnDisable()
    {
        CancelInvoke(nameof(InvokeEvent));
        Invoke(nameof(InvokeEvent),delay);
    }

    private void InvokeEvent() => OnDisableCallback?.Invoke();

}
