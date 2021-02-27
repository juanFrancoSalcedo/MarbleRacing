using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class PoolParticles : Singleton<PoolParticles>
{
    [SerializeField] private ParticlesSettings prefab;
    [SerializeField] private int countPrefabs = 1;
    private int indexCurrentParticle =0;
    private List<ParticlesSettings> particlesElements = new List<ParticlesSettings>();
    private GameObject parent;
    [SerializeField] private bool stopByTime;
    [SerializeField] private float timeStop;

    void Start()
    {
        parent = transform.gameObject;
        SetupPool();
    }

    void SetupPool()
    {
        if (countPrefabs != 0)
        {
            for (int i = 0; i < countPrefabs; i++)
            {
                ParticlesSettings dam = Instantiate(prefab);
                particlesElements.Add(dam);
                particlesElements[i].transform.SetParent(parent.transform);
                particlesElements[i].gameObject.SetActive(false);
            }
        }
        prefab.gameObject.SetActive(false);
    }

    public void PlayCurrentParticles()
    {
        particlesElements[indexCurrentParticle].gameObject.SetActive(true);
        particlesElements[indexCurrentParticle].PlayParticles();
        CheckStopByTime();
        IncreaeIndex();
    }

    public void PlayCurrentParticles(Vector3 positionParti)
    {
        particlesElements[indexCurrentParticle].gameObject.SetActive(true);
        particlesElements[indexCurrentParticle].PlayParticles();
        particlesElements[indexCurrentParticle].transform.position = positionParti;
        CheckStopByTime();
        IncreaeIndex();
    }

    public void ActiveSearchParticles(Vector3 positionParti)
    {
        if (prefab.GetComponent<DisableByTime>())
        {
            Debug.LogError("it is possible that the particles don't work because it doesn't have DisableByTime component");
        }

        foreach (ParticlesSettings parti in particlesElements)
        {
            if (!parti.gameObject.activeInHierarchy)
            {
                parti.gameObject.SetActive(true);
                parti.PlayParticles();
                parti.transform.position = positionParti;
                CheckStopByTime(parti);
                break;
            }
        }
    }

    void CheckStopByTime()
    {
        if (stopByTime)
        {
            particlesElements[indexCurrentParticle].Invoke("StopParticles", timeStop);
        }
    }

    void CheckStopByTime(ParticlesSettings _particle)
    {
        if (stopByTime)
        {
            _particle.Invoke("StopParticles", timeStop);
        }
    }

    void IncreaeIndex()
    {
        indexCurrentParticle++;
        if (indexCurrentParticle >= particlesElements.Count)
        {
            indexCurrentParticle = 0;
        }
    }
}


