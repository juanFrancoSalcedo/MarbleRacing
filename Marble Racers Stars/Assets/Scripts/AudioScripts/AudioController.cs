using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using MyBox;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSourceComp = null;
    //public GameObject shotObj;
    [SerializeField] private RaceController raceControl = null;

    [Header("~~~~~Clips~~~~~~~")]

    [SerializeField] AudioClip musicFirst = null;
    [SerializeField] AudioClip musicSecond = null;
    [SerializeField] AudioClip musicSixth = null;


    private void OnValidate()
    {
        if(raceControl == null)
        raceControl = GameObject.FindObjectOfType<RaceController>();
    }

    void Start()
    {
        //PlayerPrefs.GetInt(KeyStorage.SOUND_SETTING_I, 0)
        audioSourceComp = GetComponent<AudioSource>();
        raceControl.OnPlayerArrived += PlayerArriveMusic;
    }

    public void PlayerArriveMusic(int _positionPlayer)
    {
        if (_positionPlayer == 1)
        {
            audioSourceComp.clip = musicFirst;
            audioSourceComp.Play();
        }
        else if (_positionPlayer > 1 && _positionPlayer < 6)
        {
            audioSourceComp.clip = musicSecond;
            audioSourceComp.Play();
        }
        else if (_positionPlayer >= 6)
        {
            audioSourceComp.clip = musicSixth;
            audioSourceComp.Play();
        }
    }
}
