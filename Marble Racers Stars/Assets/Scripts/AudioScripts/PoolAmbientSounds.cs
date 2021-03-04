using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolAmbientSounds : MonoBehaviour
{
    [SerializeField] private int numberElements =4;
    private static PoolAmbientSounds soundPool;
    List<SoundShot> listShots = new List<SoundShot>();
    [SerializeField] private SoundShot prefabShot;
    [SerializeField] List<SoundSettingsShot> listSounds;
    
    private void Awake() => listShots.Add(prefabShot);

    public static PoolAmbientSounds GetInstance()
    {
        if (soundPool == null)
            soundPool = GameObject.FindObjectOfType<PoolAmbientSounds>();
        return soundPool;
    }
    
    public void PushShoot(SoundType typeSound, Vector3 _position)
    {
        if (listShots.Count < numberElements)
        {
            SoundShot soundLanding = Instantiate(prefabShot, transform);
            listShots.Add(soundLanding);
            PlaySoundOfShot(soundLanding,_position, GetClipInList(typeSound));
        }
        else
        {
            foreach (SoundShot shotStored in listShots)
            {
                if (!shotStored.gameObject.activeInHierarchy)
                {
                    PlaySoundOfShot(shotStored, _position,GetClipInList(typeSound));
                    break;
                }
            }
        }
    }

    public void PushShoot(SoundType typeSound, Vector3 _position, bool isVisible)
    {
        if (!isVisible) { return; }
        if (listShots.Count < numberElements)
        {
            SoundShot soundLanding = Instantiate(prefabShot, transform);
            listShots.Add(soundLanding);
            PlaySoundOfShot(soundLanding, _position, GetClipInList(typeSound));
        }
        else
        {
            foreach (SoundShot shotStored in listShots)
            {
                if (!shotStored.gameObject.activeInHierarchy)
                {
                    PlaySoundOfShot(shotStored, _position, GetClipInList(typeSound));
                    break;
                }
            }
        }
    }

    public AudioClip GetClipInList(SoundType _typeSound)
    {
        AudioClip clipA = null;
        foreach (var item in listSounds)
        {
            if (_typeSound == item.soundFeature)
            {
                clipA = item.clipSound;
                break;
            }
        }
        return clipA;
    }

    private void PlaySoundOfShot(SoundShot shotObj, Vector3 posShot, AudioClip clipShot)
    {
        shotObj.GetComponent<DisableByTime>().SetTimeDisable(clipShot.length);
        shotObj.gameObject.SetActive(true);
        shotObj.audSource.spatialBlend = (posShot == Vector3.zero) ? 0f : 1f;
        shotObj.audSource.clip = clipShot;
        shotObj.PlaySound();
        shotObj.transform.position = posShot;
    }
}

[System.Serializable]
public struct SoundSettingsShot
{
    public SoundType soundFeature;
    public AudioClip clipSound;
}

public enum SoundType
{
    CollisionMarbleMarble,
    CollisionMarbleTrack,
    ExploPow,
    FreezePow,
    EnlargePow,
    BoxPowerUp,
    RestoreSize,
    Accelerator,
    Respawn,
    Road,
    RoadBig,
}
