using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using System.Collections.Generic;

namespace HYPERMAGE.Managers;

public static class ProjectileManager
{
    public static readonly List<Projectile> projectiles = [];

    public static void AddProjectile(Projectile projectile)
    {
        projectiles.Add(projectile);
    }

    public static void UpdateProjectiles(Player player)
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            if (i > 100)
            {
                projectiles.Remove(projectiles[i]);
            }
        }

        foreach (var projectile in projectiles)
        {
            projectile.Update(player);
        }

        projectiles.RemoveAll(projectile => !projectile.active);
    }

    public static void Update(Player player)
    {
        UpdateProjectiles(player);
    }

    public static void Draw()
    {
        foreach (var projectile in projectiles)
        {
            projectile.Draw();
        }
    }
}