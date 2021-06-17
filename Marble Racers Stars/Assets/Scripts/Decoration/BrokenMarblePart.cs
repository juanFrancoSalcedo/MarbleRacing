using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenMarblePart : MonoBehaviour
{
    Rigidbody m_rb => GetComponent<Rigidbody>();
    Collider m_collider => GetComponent<Collider>();
    Renderer m_renderer => GetComponent<Renderer>();

    public void SetMaterial(Material mat) 
    {
        m_renderer.material = mat;
    }

    public void PartRacing() 
    {
        m_rb.isKinematic = true;
        m_collider.isTrigger = true;
    }

    public void BrokePart() 
    {
        m_rb.isKinematic = false;
        m_collider.isTrigger = false;
        if(transform.parent != null)
            Destroy(transform.parent.gameObject,4f);
    }
}
