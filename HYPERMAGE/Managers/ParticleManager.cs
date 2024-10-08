using HYPERMAGE.Particles;
using System.Collections.Generic;

namespace HYPERMAGE;

//from https://github.com/LubiiiCZ/DevQuickie/tree/master/Quickie003-ParticleSystem
public static class ParticleManager
{
    private static readonly List<Particle> _particles = [];

    public static void AddParticle(Particle p)
    {
        _particles.Add(p);
    }

    public static void UpdateParticles()
    {
        for (int i = 0; i < _particles.Count; i++)
        {
            if (i > 100)
            {
                _particles.Remove(_particles[i]);
            }
        }

        foreach (var particle in _particles)
        {
            particle.Update();
        }

        _particles.RemoveAll(p => p.isFinished);
    }

    public static void Update()
    {
        UpdateParticles();
    }

    public static void Draw()
    {
        foreach (var particle in _particles)
        {
            particle.Draw();
        }
    }
}