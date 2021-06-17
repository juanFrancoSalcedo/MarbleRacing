using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableByTime : MonoBehaviour
{
    [SerializeField] protected bool beginOnEnable;
    public float timeDisab;

    protected void OnEnable()
    {
        if (beginOnEnable)
             BeginDisableTime();
    }

    public void SetTimeDisable(float timeNew)
    {
        timeDisab = timeNew;
    }

     public void BeginDisableTime()
     {
         Invoke("DisableThisObject", timeDisab);
     }

     protected void DisableThisObject()
     {
         gameObject.SetActive(false);
     }

}

