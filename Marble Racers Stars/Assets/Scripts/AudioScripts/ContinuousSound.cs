using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;


public class ContinuousSound : MonoBehaviour
{
    [SerializeField] Marble marbleParent;
    AudioSource audioSourceCompo;
    bool onAmbientTrackPart = false;

    void Start()
    {
        if (marbleParent.justVisualAward)
        {
            gameObject.SetActive(false);
            return;
        }
        audioSourceCompo = GetComponent<AudioSource>();
        audioSourceCompo.clip = PoolAmbientSounds.GetInstance().GetClipInList(SoundType.Road);
        marbleParent.OnTrackSpeed += SetPitchSound;
        marbleParent.OnTheTrack += PlayPauseSound;
        marbleParent.OnPowerUpObtained += ChangeClipLarge;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            onAmbientTrackPart = true;
        }
    }

    void SetPitchSound(float speedMarb)
    {
        if (!audioSourceCompo.enabled) { return; }
        float pitchResult = (speedMarb > 1)? Mathf.Log10(speedMarb/2) : 0 ;
        audioSourceCompo.pitch = pitchResult;
    }

    void PlayPauseSound(bool inTrack)
    {
        if (inTrack && marbleParent.renderCompo.isVisible)
        {
            audioSourceCompo.enabled = true;
            audioSourceCompo.Play();
        }
        else
        {
            audioSourceCompo.UnPause();
            audioSourceCompo.enabled = false;
        }
    }

    void ChangeClipLarge(PowerUpType pow)
    {
        if (pow == PowerUpType.Enlarge)
        {
            audioSourceCompo.clip = PoolAmbientSounds.GetInstance().GetClipInList(SoundType.RoadBig);
            if(!onAmbientTrackPart)
                RestoreClip(Constants.timeBigSize);
        }
    }

    public void SetSoundAmbient(bool enter, AudioClip clipNew) 
    {
        onAmbientTrackPart = enter;
        if (enter) 
        {
            audioSourceCompo.clip = clipNew;
            RestoreClip();
        }
    }

    async void RestoreClip() 
    {
        while (onAmbientTrackPart)
        {
            await Task.Yield();
        }
        audioSourceCompo.clip = PoolAmbientSounds.GetInstance().GetClipInList(SoundType.Road);
        return;
    }

    async void RestoreClip(float _time)
    {
        await Task.Delay(System.TimeSpan.FromSeconds(_time));
        audioSourceCompo.clip = PoolAmbientSounds.GetInstance().GetClipInList(SoundType.Road);
        return;
    }
}
