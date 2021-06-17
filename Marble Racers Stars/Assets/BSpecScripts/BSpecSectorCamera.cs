using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;

public class BSpecSectorCamera : MonoBehaviour
{
    public CinemachineVirtualCamera compVirtual { get; private set; }
    public TriggerDetector detectorSector = null;
    [ConditionalField(nameof(detectorSector),true)] [SerializeField] private float minDist =90;
    [SerializeField] private BSpectCameraController controller = null;
    bool enterByDistance = false;
    public event System.Func<BSpecSectorCamera,int> onMarbleTargetEntered; 

    private void Awake()
    {
        compVirtual = GetComponent<CinemachineVirtualCamera>();
    }
    private void OnEnable()
    {
        if (detectorSector != null)
            detectorSector.OnTriggerEntered += MarbleTargetEnter;
        controller.onPriorityChanged += RestorePriorityZero;
    }

    private void OnDisable()
    {
        if (detectorSector != null)
            detectorSector.OnTriggerEntered -= MarbleTargetEnter;
        controller.onPriorityChanged -= RestorePriorityZero;
    }

    private void Update()
    {
        if (detectorSector == null && controller.MarbleTarget != null)
        {
            MarbleTargetEnter();
        }
    }

    void SwitchAiming()
    {
        compVirtual.GetCinemachineComponent<CinemachineComposer>().enabled = false;
    }

    private void MarbleTargetEnter(Transform other) 
    {
        if (ReferenceEquals(other.gameObject, controller.MarbleTarget.gameObject) && controller.Mode == BSpecMode.FreeMode)
            compVirtual.Priority = onMarbleTargetEntered?.Invoke(this) ?? 0;
    }

    private void MarbleTargetEnter()
    {
        if (Vector3.Distance(transform.position, controller.MarbleTarget.transform.position) < minDist)
        {
            if (!enterByDistance && controller.Mode == BSpecMode.FreeMode)
            {
                compVirtual.Priority = onMarbleTargetEntered?.Invoke(this) ?? 0;
                enterByDistance = true;
            }
        }
        else
        {
            enterByDistance = false;
        }
    }

    private void RestorePriorityZero(BSpecSectorCamera sectorOther) 
    {
        if (sectorOther == null || !ReferenceEquals(sectorOther, this))
            compVirtual.Priority = 0; 
    }
}
