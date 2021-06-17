using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using Cinemachine;

public class CameraMiniMap : Singleton<CameraMiniMap>
{
    public event System.Action onChangedMiniMap;
    public CinemachineVirtualCamera cv;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        onChangedMiniMap?.Invoke();
    }

    public void ChangeMiniMap()
    {
        switch (cv.m_Lens.OrthographicSize)
        {
            case 50:
                cv.m_Lens.OrthographicSize = 200;
                onChangedMiniMap?.Invoke();
                break;

            case 200:
                cv.m_Lens.OrthographicSize = 600;
                onChangedMiniMap?.Invoke();
                break;

            case 600:
                cv.m_Lens.OrthographicSize = 50;
                onChangedMiniMap?.Invoke();
                break;

        }
    }
}
