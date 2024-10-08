using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using System.Collections.Generic;

namespace HYPERMAGE.Managers;

public static class MobManager
{
    public static readonly List<Mob> mobs = [];

    public static void AddMob(Mob mob)
    {
        mobs.Add(mob);
    }

    public static void UpdateMobs(Player player)
    {
        foreach (var mob in mobs)
        {
            mob.Update(player);
        }

        mobs.RemoveAll(mob => !mob.active);
    }

    public static void Update(Player player)
    {
        UpdateMobs(player);
    }

    public static void Draw()
    {
        foreach (var mob in mobs)
        {
            mob.Draw();
        }
    }
}