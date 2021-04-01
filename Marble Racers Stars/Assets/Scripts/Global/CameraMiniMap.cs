using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;

public class CameraMiniMap : Singleton<CameraMiniMap>
{
    public event System.Action onChangedMiniMap;
    public Camera cameraComponent => GetComponent<Camera>();
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        onChangedMiniMap?.Invoke();
    }

    public void ChangeMiniMap()
    {
        switch (cameraComponent.orthographicSize)
        {
            case 50:
                cameraComponent.orthographicSize = 200;
                onChangedMiniMap?.Invoke();
                break;

            case 200:
                cameraComponent.orthographicSize = 600;
                onChangedMiniMap?.Invoke();
                break;

            case 600:
                cameraComponent.orthographicSize = 50;
                onChangedMiniMap?.Invoke();
                break;

        }
    }
}
