using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioClip clipAudioButton;
    
    public void SendSound()
    {
        PoolButtonSounds.GetInstance().PushShoot(clipAudioButton);
    }
}
