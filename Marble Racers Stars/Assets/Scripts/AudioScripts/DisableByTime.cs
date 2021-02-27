using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableByTime : MonoBehaviour
{
    [SerializeField] private bool beginOnEnable;
    public float timeDisab;

    private void OnEnable()
    {
        if (beginOnEnable)
        {
             BeginDisableTime();
        }
    }

    public void SetTimeDisable(float timeNew)
    {
        timeDisab = timeNew;
    }

     public void BeginDisableTime()
     {
         Invoke("DisableThisObject", timeDisab);
     }

     private void DisableThisObject()
     {
         gameObject.SetActive(false);
     }

}

