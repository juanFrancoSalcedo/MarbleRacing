using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousSound : MonoBehaviour
{
    [SerializeField] Marble marbleParent;
    AudioSource audioSourceCompo;

    void Start()
    {
        if (marbleParent.justVisualAward)
        {
            gameObject.SetActive(false);
            return;
        }
        audioSourceCompo = GetComponent<AudioSource>();
        audioSourceCompo.clip = RacersSettings.GetInstance().roadClip;
        marbleParent.OnTrackSpeed += SetPitchSound;
        marbleParent.OnTheTrack += PlayPauseSound;
        marbleParent.OnPowerUpObtained += ChangeClipLarge;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            audioSourceCompo.enabled = false;
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
            audioSourceCompo.clip = RacersSettings.GetInstance().bigSizeRoadClip;
            Invoke("RestoreClip",Constants.timeBigSize);
        }
    }

    void RestoreClip() => audioSourceCompo.clip = RacersSettings.GetInstance().roadClip;
}
