using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour
{
    public Sector nextSector;
    public TriggerDetector triggerDetector { get; set; }
    //public bool sectorForLeaderBoard;
    public float distanceBetweenNext { get; set;}
    public Vector3 positionLessZScale { get; set; }

    private void Start()
    {
        triggerDetector = GetComponent<TriggerDetector>();
        triggerDetector.OnTriggerEntered += AddMarbleEnter;
        distanceBetweenNext = Vector3.Distance(transform.position,nextSector.transform.position);
    }

    public Vector3 GetPositionCollisionFace()
    {
        float distanceFromBehind = transform.localScale.z / 2;
        positionLessZScale = transform.position - new Vector3(0,0, distanceFromBehind);
        return positionLessZScale;
    }

    private void AddMarbleEnter(Transform other)
    {
        if (!other.GetComponent<Marble>()) return;
        Marble entered = other.GetComponent<Marble>();
        entered.AddDistance(this);
    }
}
