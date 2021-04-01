using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrackerSetter : MonoBehaviour
{
    IEnumerator Start()
    {
        while (RaceController.Instance.marblePlayerInScene == null)
        {
            yield return null;
        }
        GetComponent<CinemachineVirtualCamera>().LookAt = RaceController.Instance.marblePlayerInScene.transform;
        GetComponent<CinemachineVirtualCamera>().Follow = RaceController.Instance.marblePlayerInScene.transform;

        if (RacersSettings.GetInstance().legaueManager.Liga.GetIsQualifying()) 
        {
            RaceController.Instance.onQualifiyingCompleted += ResetTarget;
        }
    }


    private void ResetTarget() 
    {
        if (!RaceController.Instance.marblePlayerInScene.isZombieQualy) return;
        foreach (var item in RaceController.Instance.marbles)
        {
            if (!item.isZombieQualy)
            {
                GetComponent<CinemachineVirtualCamera>().LookAt = item.transform;
                GetComponent<CinemachineVirtualCamera>().Follow = item.transform;
                break;
            }
        }
    }
}
