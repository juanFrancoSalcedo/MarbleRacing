using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraOutTrack : MonoBehaviour
{
    Marble playerMarble;
    [SerializeField] Animator animatorTrafficLight;
    CinemachineVirtualCamera cameraVirtual;
    void Start() 
    {
        cameraVirtual = GetComponent<CinemachineVirtualCamera>();
        playerMarble = RaceController.Instance.marblePlayerInScene;
        playerMarble.OnRespawn += ActiveCameraOut;
    }
    void ActiveCameraOut()
    {
        animatorTrafficLight.SetBool("RaceBegin", false);
        StartCoroutine(SpeedRun());
        LoadingAnimator.Instance.AnimationInit(0.9f);
    }
    IEnumerator SpeedRun()
    {
        transform.position = playerMarble.transform.position + Vector3.up * 5f - (playerMarble.rb.velocity);

        while (playerMarble.rb.velocity.magnitude < 12f)
        {
            Vector3 targe = playerMarble.transform.position + Vector3.up * 5f -(playerMarble.rb.velocity);
            transform.position = Vector3.Slerp(transform.position, targe,Time.deltaTime);
            //transform.LookAt(playerMarble.transform.position+ playerMarble.rb.velocity);
            transform.LookAt(playerMarble.transform.position);
            yield return new WaitForEndOfFrame();
        }
        animatorTrafficLight.SetBool("RaceBegin", true);
    }
}
