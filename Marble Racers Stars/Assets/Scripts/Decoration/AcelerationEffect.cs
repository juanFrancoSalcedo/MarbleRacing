using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcelerationEffect :MonoBehaviour, IMainExpected
{
    ParticleSystem particles;
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particles.Pause();
        SubscribeToMainMenu();
    }

    public void SubscribeToMainMenu() 
    {
        MainMenuController.GetInstance().OnRaceReady += ReadyToPlay;
    }

    public void ReadyToPlay()
    {
        if (RaceController.Instance.marblePlayerInScene != null)
            RaceController.Instance.marblePlayerInScene.onForceApplied += ActiveParticle;
    }
    

    void ActiveParticle()
    {
        particles.Play();
    }
}
