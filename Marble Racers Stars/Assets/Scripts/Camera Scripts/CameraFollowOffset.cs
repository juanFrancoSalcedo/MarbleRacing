using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollowOffset : MonoBehaviour
{
    CinemachineVirtualCamera m_vCam;
    CinemachineTransposer m_transposer;
    void Start()
    {
        m_vCam = GetComponent<CinemachineVirtualCamera>();
        m_transposer = m_vCam.GetCinemachineComponent<CinemachineTransposer>();
    }

    Vector3 startPos;
    Vector3 sumPos;
    Vector3 bufferInitOffset;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            bufferInitOffset = m_transposer.m_FollowOffset;
        }

        if (Input.GetMouseButton(0))
        {
            sumPos = (Input.mousePosition- new Vector3(Screen.width/2,0,Screen.height/2)).normalized;
            float dot = Mathf.Clamp(Vector3.Dot(Vector3.one, sumPos), -1, 1);
            m_transposer.m_FollowOffset = new Vector3((sumPos.x*dot*200),bufferInitOffset.y, (sumPos.y * dot*200));
        }

    }
}
