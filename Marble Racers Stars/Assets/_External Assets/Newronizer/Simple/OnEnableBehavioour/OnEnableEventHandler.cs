using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEnableCallback;
    [SerializeField] private float delay =0;

    private void OnEnable()
    {
        CancelInvoke(nameof(InvokeEvent));
        Invoke(nameof(InvokeEvent), delay);
    }

    private void InvokeEvent() => OnEnableCallback?.Invoke();
}
