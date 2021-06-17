using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyMaterialHandler : MonoBehaviour
{
    Renderer m_renderer;
    Material matBuff;
    void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    public void RestoreShader() 
    {
        m_renderer.material.SetFloat("_Cutout", 1);
        //m_renderer.sharedMaterial.SetFloat("_Cutout",1);
    }

    float hermo;
    public void UpdateFrictionDirty(float amount)
    {
        m_renderer.material.SetFloat("_Cutout", amount);
    }
}
