using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class TriggerSoundSetter : MonoBehaviour
{
    TriggerDetector detector { get { return GetComponent<TriggerDetector>(); }  set { detector = value; } }
    [SerializeField] bool pushSound;
    [ConditionalField(nameof(pushSound))] [SerializeField] AudioClip clipContinuous;


    private void Start()
    {
        detector.OnTriggerEntered += SetSoundContinuous;
    }


    private void SetSoundContinuous(Transform other) 
    {
        other.transform.GetComponentInChildren<ContinuousSound>().SetSoundAmbient(pushSound,clipContinuous);
    }

}
