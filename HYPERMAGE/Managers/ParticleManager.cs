using HYPERMAGE.Particles;
using System.Collections.Generic;
using System.Linq;

namespace HYPERMAGE;
public static class ParticleManager
{
    private static readonly List<Particle> particles = [];
    private static readonly List<TextPopup> textPopups = [];

    public static void Clear()
    {
        particles.Clear();
        textPopups.Clear();
    }
    public static void AddParticle(Particle particle)
    {
        particles.Add(particle);
    }
    public static void AddTextPopup(TextPopup textPopup)
    {
        textPopups.Add(textPopup);
    }
    public static void UpdateParticles()
    {
        foreach (var particle in particles.ToList())
        {
            particle.Update();
        }

        particles.RemoveAll(p => p.isFinished);
    }

    public static void UpdateTextPopups()
    {
        foreach (var textPopup in textPopups.ToList())
        {
            textPopup.Update();
        }

        textPopups.RemoveAll(p => p.isFinished);
    }

    public static void Update()
    {
        UpdateParticles();
        UpdateTextPopups();
    }

    public static void Draw()
    {
        foreach (var particle in particles.ToList())
        {
            particle.Draw();
        }

        foreach (var textPopup in textPopups.ToList())
        {
            textPopup.Draw();
        }
    }
}