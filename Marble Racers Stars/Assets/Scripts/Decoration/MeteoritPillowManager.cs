using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MeteoritPillowManager : MonoBehaviour
{

    [Range(0.001f,1f)]
    [SerializeField] float timeReleasePillow = 0.3f;
    [SerializeField] Pillow[] pillowMeteors;
    [SerializeField] TriggerDetector interruptor;
    public System.Action OnRainEnded;
    void Start()
    {
        interruptor.OnTriggerEntered += BeginPillowRain;
        foreach (Pillow pMeteor in pillowMeteors)
        {
            pMeteor.gameObject.SetActive(false);
        }
    }

    void BeginPillowRain(Transform other)
    {
        StartCoroutine(RainPillowDelay());
    }

    private IEnumerator RainPillowDelay()
    {
        foreach (Pillow pMeteor in pillowMeteors)
        {
            if (pMeteor != null)
            {
                pMeteor.ThrowPillow();
                pMeteor.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(timeReleasePillow);
        }

        yield return new WaitForSeconds(2);
        OnRainEnded?.Invoke();
    }
    
}
