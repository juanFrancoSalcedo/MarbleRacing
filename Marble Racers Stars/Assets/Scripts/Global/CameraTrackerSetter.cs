using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrackerSetter : MonoBehaviour
{
    [Tooltip("optional")]
    [SerializeField] private BSpectCameraController controller = null;
    IEnumerator Start()
    {
        if (controller != null)
            controller.onTargetChanged += OnManagerChangeTarget;
        while (RaceController.Instance.marblePlayerInScene == null)
        {
            yield return null;
        }
        GetComponent<CinemachineVirtualCamera>().LookAt = RaceController.Instance.marblePlayerInScene.transform;
        GetComponent<CinemachineVirtualCamera>().Follow = RaceController.Instance.marblePlayerInScene.transform;

        if (LeagueManager.LeagueRunning.GetIsQualifying()) 
            RaceController.Instance.onQualifiyingCompleted += ResetTarget;
    }


    private void ResetTarget() 
    {
        if (!RaceController.Instance.marblePlayerInScene.isZombieQualy)
        {
            return;
        } 
        foreach (var item in RaceController.Instance.marbles)
        {
            if (!item.isZombieQualy && item.gameObject.activeInHierarchy)
            {
                GetComponent<CinemachineVirtualCamera>().LookAt = item.transform;
                GetComponent<CinemachineVirtualCamera>().Follow = item.transform;
                break;
            }
        }
    }

    public void SetTarget(Transform other) 
    {
        GetComponent<CinemachineVirtualCamera>().LookAt = other;
        GetComponent<CinemachineVirtualCamera>().Follow = other;
    }

    private void OnManagerChangeTarget(Transform target) 
    {
        SetTarget(target);
    }
}
