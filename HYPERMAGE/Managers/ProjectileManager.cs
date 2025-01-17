using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using System.Collections.Generic;
using System.Linq;

namespace HYPERMAGE.Managers;

public static class ProjectileManager
{
    public static readonly List<Projectile> projectiles = [];
    public static void AddProjectile(Projectile projectile)
    {
        projectiles.Add(projectile);
    }

    public static void UpdateProjectiles()
    {
        foreach (var projectile in projectiles.ToList())
        {
            projectile.Update();
        }

        projectiles.RemoveAll(projectile => !projectile.active);
    }

    public static void Update()
    {
        UpdateProjectiles();
    }

    public static void Draw()
    {
        foreach (var projectile in projectiles.ToList())
        {
            projectile.Draw();
        }
    }
}