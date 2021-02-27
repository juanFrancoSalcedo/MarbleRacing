using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowPart : MonoBehaviour
{
    private Pillow pillowManager;
    Collider colliderPart;
    public bool attackingPart { get; set; } = false;
    public string damagetag { get; set;}
    bool staticElement;
    Rigidbody rb;
    [HideInInspector]
    public bool isMeteor;
    CollisionDetector detectorCollision;

    void Awake()
    {
        colliderPart = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        detectorCollision = GetComponent<CollisionDetector>();
        if (rb.isKinematic)
        {
            staticElement = true;
        }
    }

    private void OnEnable()
    {
        detectorCollision.OnCollisionEntered += CollisionWithMarble;
    }

    public void SetPillowManager(Pillow newManager, string tagEnemy, bool _istTrowable)
    {
        isMeteor = _istTrowable;
        pillowManager = newManager;
        pillowManager.OnPillowAttacking += ActiveAttacking;
        pillowManager.OnPillowDroped += PillowWasDroped;
        pillowManager.OnPillowThrown += PillowWasThrown;
        damagetag = tagEnemy;
    }

    public void ActiveAttacking(bool _isAttacking)
    {
        attackingPart = _isAttacking;

        if (_isAttacking)
            colliderPart.isTrigger = false;
        else
            colliderPart.isTrigger = true;
    }

    void PillowWasDroped()
    {
        colliderPart.isTrigger = false;
        attackingPart = false;
        if (staticElement)
        {
            rb.isKinematic = false;
        }
        rb.AddForce(Vector3.one);
    }

    void PillowWasThrown()
    {
        colliderPart.isTrigger = false;
        if (staticElement)
        {
            rb.isKinematic = false;
        }
    }

    void CollisionWithMarble(Collision col) 
    {
        PoolParticles.Instance.PlayCurrentParticles(col.contacts[0].point);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (isMeteor)
    //    {
    //        pillowManager.OnPillowAttacking -= ActiveAttacking;
    //        pillowManager.OnPillowDroped -= PillowWasDroped;
    //        pillowManager.OnPillowThrown -= PillowWasThrown;
    //        pillowManager.Invoke("DropPillow", 0.3f);
    //    }
    //}
}
