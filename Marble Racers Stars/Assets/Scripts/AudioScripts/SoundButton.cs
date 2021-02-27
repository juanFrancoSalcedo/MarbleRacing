using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SoundButton : MonoBehaviour
{
    Button buttonCompon;
    public AudioClip clipAudioButton;

    void Start()
    {
        buttonCompon = GetComponent<Button>();
        buttonCompon.onClick.AddListener(SendSound);
    }

    void SendSound()
    {
        PoolButtonSounds.GetInstance().PushShoot(clipAudioButton);
    }
}
