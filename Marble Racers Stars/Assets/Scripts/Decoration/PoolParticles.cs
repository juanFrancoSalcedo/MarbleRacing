using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class PoolParticles : Singleton<PoolParticles>
{
    private int indexCurrentParticle =0;
    [SerializeField] private List<ParticleSettings> particlesPrefabs = new List<ParticleSettings>();
    private List<ParticlesHandler> particlesElements = new List<ParticlesHandler>();



    public void ActiveSearchParticles(Vector3 positionParti, TypeParticle parType)
    {
        foreach (ParticlesHandler parti in particlesElements)
        {
            if (!parti.gameObject.activeInHierarchy)
            {
                parti.gameObject.SetActive(true);
                parti.PlayParticles();
                parti.transform.position = positionParti;
                return;
            }
        }

        ParticlesHandler dam = Instantiate(particlesPrefabs.Find(x => x.particleType == parType).prefabParticle);
        particlesElements.Add(dam);
        dam.transform.SetParent(transform);
        dam.PlayParticles();
    }
}
[System.Serializable]
public struct ParticleSettings 
{
    public TypeParticle particleType;
    public ParticlesHandler prefabParticle;
}

public enum TypeParticle 
{
    FeatherExplo,
    StarExplo
}




