using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolButtonSounds : MonoBehaviour
{
    [SerializeField] private int numberElements = 4;
    private static PoolButtonSounds soundPool;
    List<SoundShot> listOfButtonSounds = new List<SoundShot>();
    [SerializeField] private SoundShot prefabShot;

    public void Awake() => listOfButtonSounds.Add(prefabShot);

    public static PoolButtonSounds GetInstance()
    {
        if (soundPool == null)
        {
            soundPool = GameObject.FindObjectOfType<PoolButtonSounds>();
        }
        return soundPool;
    }
    
    public void PushShoot(AudioClip _clip)
    {
        if (listOfButtonSounds.Count < numberElements)
        {
            SoundShot soundLanding = Instantiate(prefabShot, transform);
            listOfButtonSounds.Add(soundLanding);
            PlaySoundOfShot(soundLanding, _clip);
        }
        else
        {
            foreach (SoundShot shotStored in listOfButtonSounds)
            {
                if (!shotStored.gameObject.activeInHierarchy)
                {
                    PlaySoundOfShot(shotStored,_clip);
                    break;
                }
            }
        }
    }

    private void  PlaySoundOfShot(SoundShot shotObj, AudioClip clipShot)
    {
        shotObj.gameObject.SetActive(true);
        shotObj.audSource.clip = clipShot;
        shotObj.PlaySound();
    }
}
