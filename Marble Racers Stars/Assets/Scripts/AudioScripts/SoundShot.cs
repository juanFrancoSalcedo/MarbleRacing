using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundShot : MonoBehaviour
{
    public AudioSource audSource { get; private set; }

    private void OnEnable()
    {
        audSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audSource.Play();
    }
}
