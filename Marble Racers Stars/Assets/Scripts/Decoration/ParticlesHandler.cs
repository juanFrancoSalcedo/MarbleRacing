using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesHandler : DisableByTime
{
    ParticleSystem[] particles =>  GetComponentsInChildren<ParticleSystem>();
    public void PlayParticles()
    {
        foreach (var p in particles)
        {
            p.Play();
        }
    }

    public void SetColorMainParticles(Color _color)
    {
        foreach (var p in particles)
        {
            var main = p.main;
            main.startColor = _color;
        }
    }

    public void PauseParticles()
    {
        foreach (var p in particles)
            p.Pause();
    }

    public void StopParticles()
    {
        foreach (var p in particles)
            p.Stop();
    }

    public bool isPlaying 
    {
        get {return particles[0].isPlaying;}
        private set{ }
    }
}
