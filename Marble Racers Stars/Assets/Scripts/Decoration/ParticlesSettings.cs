using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesSettings : MonoBehaviour
{
    public void PlayParticles()
    {
        var particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var p in particles)
        {
            p.Play();
        }
    }

    public void SetColorMainParticles(Color _color)
    {
        var particles = GetComponentsInChildren<ParticleSystem>();
        
        foreach (var p in particles)
        {
            var main = p.main;
            main.startColor = _color;
        }
    }

    public void PauseParticles()
    {
        var particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var p in particles)
            p.Pause();
    }

    public void StopParticles()
    {
        var particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var p in particles)
            p.Stop();
    }
}
