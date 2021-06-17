using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;

public class CameraShake : Singleton<CameraShake>
{
    private CinemachineVirtualCamera cam = null;
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake()
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 2;
        StartCoroutine(ShakeByTime(0.5f));
    }

    private IEnumerator ShakeByTime(float limitTime)
    {
        float timer =0;
        while (timer < limitTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }
}
